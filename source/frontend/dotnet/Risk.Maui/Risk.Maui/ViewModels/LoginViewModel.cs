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
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private ValidatableObject<string> _usuario = new();

    [ObservableProperty]
    private ValidatableObject<string> _clave = new();

    [ObservableProperty]
    private bool _isValid;

    public LoginViewModel(ISettingsService settingsService, IAppEnvironmentService appEnvironmentService, IDialogService dialogService, INavigationService navigationService) : base(navigationService)
    {
        _settingsService = settingsService;
        _appEnvironmentService = appEnvironmentService;
        _dialogService = dialogService;

        AddValidations();
    }

    [RelayCommand]
    private async Task OpenLoading()
    {
        var pop = await _dialogService.ShowLoadingAsync("Cargando");
        await Task.Delay(5000);
        pop.Close();
    }

    [RelayCommand]
    private void Validate()
    {
        IsValid = Usuario.Validate() && Clave.Validate();
    }

    [RelayCommand]
    private async Task SignInAsync()
    {
        var pop = await _dialogService.ShowLoadingAsync("Cargando");
        SesionRespuesta sesionRespuesta = await _appEnvironmentService.AutApi.IniciarSesionAsync(_settingsService.DeviceToken, null, new IniciarSesionRequestBody
        {
            Usuario = Usuario.Value,
            Clave = Clave.Value
        });
        pop.Close();

        if (sesionRespuesta.Codigo.Equals(RiskConstants.CODIGO_OK))
        {
            _settingsService.AccessToken = sesionRespuesta.Datos.AccessToken;
            _settingsService.RefreshToken = sesionRespuesta.Datos.RefreshToken;

            _appEnvironmentService.UpdateDependencies(sesionRespuesta.Datos.AccessToken);

            _settingsService.IsUserLoggedIn = true;

            await NavigationService.NavigateToAsync("//MainPage");
        }
        else
        {
            await _dialogService.ShowAlertAsync(sesionRespuesta.Mensaje, "Error", "Ok");
        }
    }

    private void AddValidations()
    {
        Usuario.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A username is required." });
        Clave.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
    }
}
