using CommunityToolkit.Maui.Views;
using Risk.Maui.Resources.Languages;

namespace Risk.Maui.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title = default(string), string buttonLabel = default(string))
        {
            return Application.Current.MainPage.DisplayAlert(string.IsNullOrEmpty(title) ? AppResources.AlertDefaultTitle : title, message, string.IsNullOrEmpty(buttonLabel) ? AppResources.AlertDefaultButtonLabel : buttonLabel);
        }

        public Task<SpinnerPopup> ShowLoadingAsync(string message)
        {
            var popup = new SpinnerPopup();
            Application.Current.MainPage.ShowPopup(popup);
            return Task.FromResult(popup);
        }
    }
}