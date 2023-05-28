using Risk.API.Client.Model;
using Risk.Common.Helpers;
using Risk.Maui.Services.AppEnvironment;
using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Settings;

namespace Risk.Maui;

public partial class App : Application
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;
    private readonly IDialogService _dialogService;

    public App(ISettingsService settingsService, IAppEnvironmentService appEnvironmentService, IDialogService dialogService)
    {
        _settingsService = settingsService;
        _appEnvironmentService = appEnvironmentService;
        _dialogService = dialogService;

        InitializeComponent();

        InitApp();

        MainPage = new AppShell();
    }

    private void InitApp()
    {
    }

    protected override async void OnStart()
    {
        await CheckAppVersion();
        await RegisterDevice();

        var status = await CheckAndRequestLocationPermission();
        _settingsService.AllowGpsLocation = (status == PermissionStatus.Granted);

        if (_settingsService.AllowGpsLocation)
        {
            await GetGpsLocation();
            await SendCurrentLocation();
        }

        base.OnStart();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
    }

    protected override void OnResume()
    {
        base.OnResume();
    }

    private async Task CheckAppVersion()
    {
        AplicacionPaginaRespuesta respuestaListarAplicaciones = null;
        try
        {
            respuestaListarAplicaciones = await _appEnvironmentService.GenApi.ListarAplicacionesAsync(null, _settingsService.ApiKey);
        }
        catch (Exception)
        {
            await _dialogService.ShowAlertAsync("La aplicación no está activa.", "Oops!", "Ok");
            return;
        }

        if (respuestaListarAplicaciones.Codigo.Equals(RiskConstants.CODIGO_OK))
        {
            var aplicacion = respuestaListarAplicaciones.Datos.Elementos[0];

            // Valida si la aplicación está activa
            if (!aplicacion.Activo)
            {
                await _dialogService.ShowAlertAsync("La aplicación no está activa.", "Oops!", "Ok");
                return;
            }

            // Valida versión de la aplicación
            Version versionAplicacion = new Version(AppInfo.Current.VersionString);

            if (!string.IsNullOrEmpty(aplicacion.VersionMinima))
            {
                Version versionServidor = new Version(aplicacion.VersionMinima);

                switch (versionAplicacion.CompareTo(versionServidor))
                {
                    case 0:
                        //Console.Write("the same as");
                        break;
                    case 1:
                        //Console.Write("later than");
                        break;
                    case -1:
                        //Console.Write("earlier than");
                        await _dialogService.ShowAlertAsync("Es necesaria una actualización de la aplicación", "Oops!", "Ok");
                        return;
                }
            }
        }
    }

    private async Task RegisterDevice()
    {
        // Registra dispositivo
        string deviceToken = _settingsService.DeviceToken;

        // Datos del idioma y pais del dispositivo
        string idiomaIso = null;
        string paisIso = null;
        try
        {
            string infoPaisIdioma = string.Empty; //DependencyService.Get<ILocationService>().GetCountry();
            string[] infoPaisIdiomaList = infoPaisIdioma.Split('_');
            idiomaIso = (string)infoPaisIdiomaList.GetValue(0);
            paisIso = (string)infoPaisIdiomaList.GetValue(1);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        // Datos de la zona horaria del dispositivo
        string offset = null;
        try
        {
            TimeSpan ts = new DateTimeOffset(DateTime.Now).Offset;
            offset = ts.Hours + ":" + ts.Minutes;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        DatoRespuesta datoRespuesta = await _appEnvironmentService.AutApi.RegistrarDispositivoAsync(null, null, new RegistrarDispositivoRequestBody
        {
            Dispositivo = new Dispositivo
            {
                IdDispositivo = 0,
                TokenDispositivo = deviceToken,
                NombreSistemaOperativo = DeviceInfo.Current.Platform.ToString(),
                VersionSistemaOperativo = DeviceInfo.Current.VersionString,
                Tipo = TipoDispositivo.Mobile,
                VersionAplicacion = AppInfo.Current.VersionString,
                IdPaisIso2 = paisIso,
                ZonaHoraria = offset,
                IdiomaIso = idiomaIso
            }
        });

        if (datoRespuesta.Codigo.Equals(RiskConstants.CODIGO_OK))
        {
            _settingsService.DeviceToken = datoRespuesta.Datos.Contenido;
        }
    }

    public async Task<PermissionStatus> CheckAndRequestLocationPermission()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status == PermissionStatus.Granted)
            return status;

        if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
        {
            // Prompt the user to turn on in settings
            // On iOS once a permission has been denied it may not be requested again from the application
            return status;
        }

        if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
        {
            // Prompt the user with additional information as to why the permission is needed
        }

        status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

        return status;
    }

    private async Task GetGpsLocation()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request, CancellationToken.None);

            if (location != null)
            {
                _settingsService.Latitude = location.Latitude;
                _settingsService.Longitude = location.Longitude;
            }
        }
        catch (Exception ex)
        {
            if (ex is FeatureNotSupportedException || ex is FeatureNotEnabledException || ex is PermissionException)
            {
                _settingsService.AllowGpsLocation = false;
            }

            // Unable to get location
            Debug.WriteLine(ex);
        }
    }

    private async Task SendCurrentLocation()
    {
        DatoRespuesta datoRespuesta = await _appEnvironmentService.AutApi.RegistrarUbicacionAsync(null, null, new RegistrarUbicacionRequestBody
        {
            TokenDispositivo = _settingsService.DeviceToken,
            Latitud = _settingsService.Latitude,
            Longitud = _settingsService.Longitude
        });

        if (!datoRespuesta.Codigo.Equals(RiskConstants.CODIGO_OK))
        {
            Debug.WriteLine(datoRespuesta);
        }
    }
}
