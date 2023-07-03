using Risk.API.Client.Model;
using Risk.Common.Helpers;
using Risk.Maui.Services.AppEnvironment;
using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Navigation;
using Risk.Maui.Services.Settings;
using Risk.Maui.Validations;
using Risk.Maui.ViewModels.Base;

namespace Risk.Maui.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly IAppEnvironmentService _appEnvironmentService;

    [ObservableProperty]
    private ValidatableObject<string> _usuario = new();

    [ObservableProperty]
    private ValidatableObject<string> _clave = new();

    [ObservableProperty]
    private bool _isValid;

    public LoginViewModel(ISettingsService settingsService, INavigationService navigationService, IDialogService dialogService, IAppEnvironmentService appEnvironmentService) : base(settingsService, navigationService, dialogService)
    {
        _appEnvironmentService = appEnvironmentService;

        AddValidations();
    }

    [RelayCommand]
    private void Validate()
    {
        IsValid = Usuario.Validate() && Clave.Validate();
    }

    [RelayCommand]
    private async Task SignInAsync()
    {
        await IsBusyFor(
           async () =>
           {
               SesionRespuesta sesionRespuesta = await _appEnvironmentService.AutApi.IniciarSesionAsync(SettingsService.DeviceToken, null, new IniciarSesionRequestBody
               {
                   Usuario = Usuario.Value,
                   Clave = Clave.Value
               });

               if (sesionRespuesta.Codigo.Equals(RiskConstants.CODIGO_OK))
               {
                   SettingsService.AccessToken = sesionRespuesta.Datos.AccessToken;
                   SettingsService.RefreshToken = sesionRespuesta.Datos.RefreshToken;
                   SettingsService.IsUserLoggedIn = true;

                   _appEnvironmentService.UpdateDependencies(sesionRespuesta.Datos.AccessToken);

                   await NavigationService.NavigateToAsync("//MainPage");
               }
               else
               {
                   await DialogService.ShowAlertAsync(sesionRespuesta.Mensaje);
               }
           });
    }

    private void AddValidations()
    {
        Usuario.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A username is required." });
        Clave.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
    }
}
