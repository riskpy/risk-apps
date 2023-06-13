using Risk.Maui.Services.Dialog;

namespace Risk.Maui.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IDialogService _dialogService;

    public MainViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [RelayCommand]
    private async Task OpenLoading()
    {
        var pop = await _dialogService.ShowLoadingAsync("Cargando");
        await Task.Delay(5000);
        pop.Close();
    }
}
