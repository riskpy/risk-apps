using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Navigation;
using Risk.Maui.Services.Settings;

namespace Risk.Maui.ViewModels.Base;

public interface IViewModelBase : IQueryAttributable
{
    public ISettingsService SettingsService { get; }

    public INavigationService NavigationService { get; }

    public IDialogService DialogService { get; }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public bool IsBusy { get; }

    public bool IsInitialized { get; }

    Task InitializeAsync();
}