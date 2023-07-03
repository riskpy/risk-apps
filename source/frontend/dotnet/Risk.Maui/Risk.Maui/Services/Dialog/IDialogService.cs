namespace Risk.Maui.Services.Dialog
{
    public interface IDialogService
    {
        Task ShowAlertAsync(string message, string title = default(string), string buttonLabel = default(string));

        Task<SpinnerPopup> ShowLoadingAsync(string message = default(string));
    }
}