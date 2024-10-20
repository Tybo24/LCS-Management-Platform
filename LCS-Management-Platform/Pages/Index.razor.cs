using LCS_Management_Platform.Data;
using LCS_Management_Platform.Helpers;
using LCS_Management_Platform.Models;
using LCS_Management_Platform.Services;
using LCS_Management_Platform.Services.Implementations;
using LCS_Management_Platform.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using Newtonsoft.Json;
using System.Web.Http;
using static MudBlazor.CategoryTypes;

namespace LCS_Management_Platform.Pages
{
    public partial class Index()
    {
        [Inject]
        private DialogServiceHelper DialogServiceHelper { get; set; }
        MudTextField<string>? username;
        MudTextField<string>? password;
        private string searchString1 = "";
        private bool _processing;
        private System.Timers.Timer? timer;
        private EnvDataList selectedItem1 = null;
        private HashSet<EnvDataList> selectedItems = new HashSet<EnvDataList>();
        private static IEnumerable<EnvDataList> Environments = new List<EnvDataList>();

        protected override async Task OnInitializedAsync()
        {

        }

        private async Task<bool> RefreshAuthToken()
        {
            try
            {
                AppState.tokenAcquired = await AuthService.GetAuthTokenAsync(username.Value, password.Value);
            }
            catch (Exception)
            {
                throw;
            }

            return AppState.tokenAcquired;
        }

        private async Task<bool> GetEnvironmentData()
        {
            try
            {
                if (DateTime.UtcNow > AppState.waitUntil)
                {
                    var environmentIds = SettingsService.Settings.EnvironmentIds;

                    IList<EnvDataList> envData = new List<EnvDataList>();
                    List<string> missingData = new List<string>();

                    foreach (string id in environmentIds)
                    {
                        EnvironmentData envDataLocal =
                            await EnvironmentDataService.GetEnvironmentDataAsync(SettingsService.Settings.Project, id, AppState.accessToken);

                        if (envDataLocal != null)
                        {
                            List<EnvHistDataList> envHistDataList = new List<EnvHistDataList>();

                            EnvironmentHistoryData envHistDataLocal =
                                await EnvironmentDataService.GetEnvironmentHistoryDataAsync(SettingsService.Settings.Project, id, AppState.accessToken);

                            if (envHistDataLocal != null)
                            {
                                foreach (var d in envHistDataLocal.Data)
                                {
                                    envHistDataList.Add(new EnvHistDataList
                                    {
                                        Name = d.Name,
                                        TypeDisplay = d.TypeDisplay,
                                        StartDateTimeUTC = d.StartDateTimeUTC,
                                        EndDateTimeUTC = d.EndDateTimeUTC,
                                        Status = d.Status
                                    });
                                }
                            }

                            envData.Add(new EnvDataList
                            {
                                ShowDetails = false,
                                EnvironmentId = envDataLocal.Data[0].EnvironmentId,
                                EnvironmentName = envDataLocal.Data[0].EnvironmentName,
                                EnvironmentType = envDataLocal.Data[0].EnvironmentType,
                                DeploymentStatusDisplay = envDataLocal.Data[0].DeploymentStatusDisplay,
                                CurrentApplicationReleaseName = envDataLocal.Data[0].CurrentApplicationReleaseName,
                                CurrentApplicationBuildVersion = envDataLocal.Data[0].CurrentApplicationBuildVersion,
                                CurrentPlatformVersion = envDataLocal.Data[0].CurrentPlatformVersion,
                                CurrentCustomisation = DataManipHelper.CurrentCustomisation(envHistDataList),
                                EnvHistData = envHistDataList.Any() ? envHistDataList : null
                            });
                        }
                        else
                        {
                            // identify the environments we couldn't fetch data for
                            missingData.Add(id);
                        }
                    }

                    // we got all data concerned and can present it as such
                    if (envData.Count >= environmentIds.Count())
                    {
                        // final updates to list
                        envData = await DataManipHelper.ProductionEnvironmentFromList(envData);

                        Environments = envData;
                        AppState.environmentData = true;
                    }
                    // if we already have some environments but might have got less data returned because of API call issues
                    else if (Environments.Any())
                    {
                        IList<EnvDataList> updatedEnvironments = Environments.ToList(); // Convert to List for manipulation

                        // Mark environments with missing data
                        foreach (var environment in updatedEnvironments)
                        {
                            if (missingData.Contains(environment.EnvironmentId))
                            {
                                environment.DataMissing = true; // Mark as missing
                            }
                        }

                        // Replace or update successful data
                        foreach (var newEnvData in envData)
                        {
                            // Find if the environment already exists in the list
                            var existingEnv = updatedEnvironments.FirstOrDefault(env => env.EnvironmentId == newEnvData.EnvironmentId);

                            if (existingEnv != null)
                            {
                                // Replace the existing environment with new data
                                updatedEnvironments.Remove(existingEnv);
                            }

                            // Add the new environment data to the list
                            updatedEnvironments.Add(newEnvData);
                        }

                        // final updates to list
                        updatedEnvironments = await DataManipHelper.ProductionEnvironmentFromList(updatedEnvironments);

                        // Assign the updated list back to Environments
                        Environments = updatedEnvironments;
                    }

                    // TODO: Do I need to find better place for this?
                    AppState.envDataLastUpdate = DateTimeHelper.CurrentTime();
                }
                else
                {
                    await DialogServiceHelper.ShowErrorDialog("Too many requests!",
                        "Too many requests have been made recently, please wait at least 5 minutes before trying again.");
                }

            }
            catch (Exception ex)
            {
                throw;
            }

            return AppState.environmentData;
        }

        private async Task SubmitUserAndGetData()
        {
            try
            {
                _processing = true;

                if (string.IsNullOrEmpty(username.Value) || string.IsNullOrEmpty(password.Value))
                {
                    return;
                    // do nothing
                }
                else
                {
                    var user = username.Value.ToLower();

                    if (user.Contains(SettingsService.Settings.AdminUser))
                    {
                        await RefreshAuthToken();

                        if (AppState.tokenAcquired)
                        {
                            await GetEnvironmentData();

                            if (AppState.environmentData)
                            {
                                return;
                            }
                            else
                            {
                                await DialogServiceHelper.ShowErrorDialog("No data",
                                    "Unable to retrieve data. Try again later.");
                            }
                        }
                    }
                    else
                    {
                        await DialogServiceHelper.ShowErrorDialog("Error",
                            "Incorrect user or pass");
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogServiceHelper.ShowErrorDialog("Error", ex.Message);
            }
            finally
            {
                _processing = false;
                StateHasChanged();
            }
        }

        private async Task SubmitUserAndRefreshData()
        {
            try
            {
                if (!AppState.timerActive)
                {
                    _processing = true;

                    StartCountdown();

                    if (DateTime.UtcNow >= AppState.tokenExpiresAt)
                    {
                        // Dialog call makes auth call
                        var dialog = await DialogService.ShowAsync<CredentialsDialog>("Credentials expired");
                        var result = await dialog.Result;

                        if (!result.Canceled && result.Data.Equals(true))
                        {
                            await GetEnvironmentData();
                        }
                        else
                        {
                            ResetTimer();
                        }
                    }
                    else
                    {
                        await GetEnvironmentData();
                    }
                }
            }
            catch (Exception ex)
            {
                await DialogServiceHelper.ShowErrorDialog("Error", ex.Message);
            }
            finally
            {
                _processing = false;
                StateHasChanged();
            }
        }

        private async Task StartCountdown()
        {
            AppState.timerActive = true;
            AppState.timerCountdown = SettingsService.Settings.APICooldown;

            while (AppState.timerCountdown > 0)
            {
                await Task.Delay(1000); // Delay for 1 second
                AppState.timerCountdown -= 1000;

                await InvokeAsync(StateHasChanged);
            }

            AppState.timerActive = false;

            await InvokeAsync(StateHasChanged);
        }

        private void ResetTimer()
        {
            timer?.Stop();
            AppState.timerActive = false;
            AppState.timerCountdown = 0;
            InvokeAsync(StateHasChanged);
        }

        private void ShowBtnPress(string id)
        {
            EnvDataList tmpData = Environments.First(f => f.EnvironmentId == id);
            tmpData.ShowDetails = !tmpData.ShowDetails;
        }

        private bool FilterFunc1(EnvDataList env) => FilterFunc(env, searchString1);

        private bool FilterFunc(EnvDataList environment, string searchString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return true;
                if (environment.EnvironmentName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (environment.EnvironmentId.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (environment.CurrentCustomisation.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (environment.CurrentPlatformVersion.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (environment.CurrentApplicationBuildVersion.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (environment.CurrentApplicationReleaseName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if (environment.EnvironmentType.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}