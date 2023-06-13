using CommunityToolkit.Maui.Views;

namespace Risk.Maui.Services.Dialog
{
    public class DialogService : IDialogService
    {
        public Task ShowAlertAsync(string message, string title, string buttonLabel)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, buttonLabel);
        }

        public Task<SpinnerPopup> ShowLoadingAsync(string message)
        {
            var popup = new SpinnerPopup();
            Application.Current.MainPage.ShowPopup(popup);
            return Task.FromResult(popup);
        }
    }
}