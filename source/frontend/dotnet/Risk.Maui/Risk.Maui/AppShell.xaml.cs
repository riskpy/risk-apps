using Risk.Maui.Services.Navigation;

namespace Risk.Maui;

public partial class AppShell : Shell
{
    private readonly INavigationService _navigationService;

    public AppShell(INavigationService navigationService)
    {
        _navigationService = navigationService;

        InitializeRouting();
        InitializeComponent();
    }

    protected override async void OnHandlerChanged()
    {
        base.OnHandlerChanged();

        if (Handler is not null)
        {
            await _navigationService.InitializeAsync();
        }
    }

    private static void InitializeRouting()
    {
        foreach (var route in Routes.RouteTypeMap)
        {
            Routing.RegisterRoute(route.Key, route.Value);
        }
        //Routing.RegisterRoute("Filter", typeof(FiltersView));
    }
}
