using Risk.API.Client.Model;
using Risk.Common.Helpers;
using Risk.Maui.Services.AppEnvironment;
using Risk.Maui.Services.Settings;

namespace Risk.Maui;

public partial class App : Application
{
    private readonly ISettingsService _settingsService;
    private readonly IAppEnvironmentService _appEnvironmentService;

    public App(ISettingsService settingsService, IAppEnvironmentService appEnvironmentService)
    {
        _settingsService = settingsService;
        _appEnvironmentService = appEnvironmentService;

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
            Debug.WriteLine("La aplicación no está activa.");
            return;
        }

        if (respuestaListarAplicaciones.Codigo.Equals(RiskConstants.CODIGO_OK))
        {
            var aplicacion = respuestaListarAplicaciones.Datos.Elementos[0];

            // Valida si la aplicación está activa
            if (!aplicacion.Activo)
            {
                Debug.WriteLine("La aplicación no está activa.");
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
                        Debug.WriteLine("Es necesaria una actualización de la aplicación");
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
}
