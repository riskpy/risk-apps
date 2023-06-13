namespace Risk.Maui.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title, string buttonLabel);
        Task<SpinnerPopup> ShowLoadingAsync(string message);
    }
}