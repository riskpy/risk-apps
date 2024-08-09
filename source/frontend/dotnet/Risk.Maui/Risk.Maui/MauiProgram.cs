using Risk.Maui.Services.AppEnvironment;
using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Navigation;
using Risk.Maui.Services.OpenUrl;
using Risk.Maui.Services.Settings;
using epj.RouteGenerator;

namespace Risk.Maui;

[AutoRoutes("Page")]
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
                fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
                fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

        return builder.Build();
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<ISettingsService, SettingsService>();
        mauiAppBuilder.Services.AddSingleton<INavigationService, MauiNavigationService>();
        mauiAppBuilder.Services.AddSingleton<IDialogService, DialogService>();
        mauiAppBuilder.Services.AddSingleton<IOpenUrlService, OpenUrlService>();

        mauiAppBuilder.Services.AddSingleton<IAppEnvironmentService, AppEnvironmentService>(
        serviceProvider =>
        {
            var settingsService = serviceProvider.GetService<ISettingsService>();
            settingsService.ApiBasePath = GlobalSettings.Instance.ApiBasePath;
            settingsService.ApiKey = GlobalSettings.Instance.ApiKey;

            var aes = new AppEnvironmentService(settingsService.ApiBasePath,
                settingsService.ApiKey,
                settingsService.AccessToken);

            return aes;
        });

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainViewModel>();
        mauiAppBuilder.Services.AddSingleton<SampleViewModel>();
        mauiAppBuilder.Services.AddSingleton<LoginViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        mauiAppBuilder.Services.AddSingleton<SamplePage>();
        mauiAppBuilder.Services.AddSingleton<LoginPage>();

        return mauiAppBuilder;
    }
}
