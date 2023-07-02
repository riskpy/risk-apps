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
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly IDialogService _dialogService;

    public MainViewModel(ISettingsService settingsService, IAppEnvironmentService appEnvironmentService, IDialogService dialogService, INavigationService navigationService) : base(navigationService)
    {
        _settingsService = settingsService;
        _appEnvironmentService = appEnvironmentService;
        _dialogService = dialogService;
    }

    [RelayCommand]
    private async Task SignOutAsync()
    {
        var pop = await _dialogService.ShowLoadingAsync("Cargando");

        DatoRespuesta respuestaFinalizarSesion = null;
        try
        {
            respuestaFinalizarSesion = await _appEnvironmentService.AutApi.FinalizarSesionAsync(_settingsService.DeviceToken, null, new FinalizarSesionRequestBody { AccessToken = _settingsService.AccessToken });

            if (respuestaFinalizarSesion.Codigo.Equals(RiskConstants.CODIGO_OK))
            {
                _settingsService.AccessToken = string.Empty;
                _settingsService.RefreshToken = string.Empty;
                _settingsService.IsUserLoggedIn = false;

                await NavigationService.NavigateToAsync("//LoginPage");
            }
            else
            {
                await _dialogService.ShowAlertAsync("Error al finalizar sesión", "Error", "Ok");
            }
        }
        catch (ApiException ex)
        {
            if (ex.ErrorCode.Equals(401))
            {
                _settingsService.AccessToken = string.Empty;
                _settingsService.RefreshToken = string.Empty;
                _settingsService.IsUserLoggedIn = false;

                await NavigationService.NavigateToAsync("//LoginPage");
            }
            else
            {
                await _dialogService.ShowAlertAsync("Error al finalizar sesión", "Error", "Ok");
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error al finalizar sesión", "Error", "Ok");
        }

        pop.Close();
    }
}
