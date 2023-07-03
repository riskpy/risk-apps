using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Navigation;
using Risk.Maui.Services.Settings;
using Risk.Maui.ViewModels.Base;

namespace Risk.Maui.ViewModels;

public partial class SampleViewModel : ViewModelBase
{
    int count = 0;

    [ObservableProperty]
    public string message = "Click me";

    public SampleViewModel(ISettingsService settingsService, INavigationService navigationService, IDialogService dialogService) : base(settingsService, navigationService, dialogService)
    {
    }

    [RelayCommand]
    private async Task OnCounterClicked()
    {
        await IsBusyFor(
           async () =>
           {
               await Task.Delay(5000);
           }, true);


        count++;

        if (count == 1)
            Message = $"Clicked {count} time";
        else
            Message = $"Clicked {count} times";

        SemanticScreenReader.Announce(Message);
    }
}
