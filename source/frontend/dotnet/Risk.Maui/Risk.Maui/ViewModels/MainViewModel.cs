using Risk.API.Client.Client;
using Risk.API.Client.Model;
using Risk.Common.Helpers;
using Risk.Maui.Services.AppEnvironment;
using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Navigation;
using Risk.Maui.Services.Settings;
using Risk.Maui.ViewModels.Base;

namespace Risk.Maui.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IAppEnvironmentService _appEnvironmentService;

    public MainViewModel(ISettingsService settingsService, INavigationService navigationService, IDialogService dialogService, IAppEnvironmentService appEnvironmentService) : base(settingsService, navigationService, dialogService)
    {
        _appEnvironmentService = appEnvironmentService;
    }

    [RelayCommand]
    private async Task SignOutAsync()
    {
        await IsBusyFor(
           async () =>
           {
               DatoRespuesta respuestaFinalizarSesion = null;
               try
               {
                   respuestaFinalizarSesion = await _appEnvironmentService.AutApi.FinalizarSesionAsync(SettingsService.DeviceToken, null, new FinalizarSesionRequestBody { AccessToken = SettingsService.AccessToken });

                   if (respuestaFinalizarSesion.Codigo.Equals(RiskConstants.CODIGO_OK))
                   {
                       SettingsService.AccessToken = string.Empty;
                       SettingsService.RefreshToken = string.Empty;
                       SettingsService.IsUserLoggedIn = false;

                       _appEnvironmentService.UpdateDependencies(string.Empty);

                       await NavigationService.NavigateToAsync("//LoginPage");
                   }
                   else
                   {
                       await DialogService.ShowAlertAsync("Error al finalizar sesión");
                   }
               }
               catch (ApiException ex)
               {
                   if (ex.ErrorCode.Equals(401))
                   {
                       SettingsService.AccessToken = string.Empty;
                       SettingsService.RefreshToken = string.Empty;
                       SettingsService.IsUserLoggedIn = false;

                       _appEnvironmentService.UpdateDependencies(string.Empty);

                       await NavigationService.NavigateToAsync("//LoginPage");
                   }
                   else
                   {
                       await DialogService.ShowAlertAsync("Error al finalizar sesión");
                   }
               }
               catch (Exception ex)
               {
                   await DialogService.ShowAlertAsync("Error al finalizar sesión");
               }
           }, true);
    }
}
