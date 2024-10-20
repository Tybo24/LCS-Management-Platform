using LCS_Management_Platform.Shared;
using MudBlazor;

namespace LCS_Management_Platform.Helpers
{
    public class DialogServiceHelper
    {
        private readonly IDialogService _dialogService;

        public DialogServiceHelper(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public async Task ShowErrorDialog(string title, string message)
        {
            var parameters = new DialogParameters
            {
                ["ContentText"] = message
            };

            await _dialogService.ShowAsync<ErrorDialog>(title, parameters);
        }
    }

}
