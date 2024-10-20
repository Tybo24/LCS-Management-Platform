using LCS_Management_Platform.Data;
using LCS_Management_Platform.Helpers;
using LCS_Management_Platform.Services;
using LCS_Management_Platform.Services.Implementations;
using LCS_Management_Platform.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudBlazor.Services;

namespace LCS_Management_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddMudServices();
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
            builder.Services.AddTransient<IAuthService, AuthService>();
            builder.Services.AddTransient<IEnvironmentDataService, EnvironmentDataService>();
            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddScoped<DialogServiceHelper>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}