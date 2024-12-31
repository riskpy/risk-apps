using Risk.Common.Helpers;
using Risk.Maui.Models;
using Risk.Maui.Services.AppEnvironment;
using Risk.Maui.Services.Dialog;
using Risk.Maui.Services.Navigation;
using Risk.Maui.Services.Settings;
using Risk.Maui.ViewModels.Base;

namespace Risk.Maui.ViewModels
{
    public partial class CountriesViewModel : ViewModelBase
    {
        private readonly IAppEnvironmentService _appEnvironmentService;
        private readonly ObservableCollectionEx<Country> _countries = new();

        public IReadOnlyList<Country> Countries => _countries;

        private int ultimaPagina;


        public CountriesViewModel(ISettingsService settingsService, INavigationService navigationService, IDialogService dialogService, IAppEnvironmentService appEnvironmentService) : base(settingsService, navigationService, dialogService)
        {
            _appEnvironmentService = appEnvironmentService;
        }

        public override async Task InitializeAsync()
        {
            await RefreshCountries();
        }

        [RelayCommand]
        private async Task RefreshCountries()
        {
            await IsBusyFor(async () =>
            {
                ultimaPagina = 1;
                var respuesta = await _appEnvironmentService.GloApi.ListarPaisesAsync(ultimaPagina, 10, false);

                if (respuesta.Codigo.Equals(RiskConstants.CODIGO_OK))
                {
                    _countries.ReloadData(respuesta.Datos.Elementos.ConvertAll(x => new Country
                    {
                        IsoAlpha2 = x.IsoAlpha2,
                        Nombre = x.Nombre,
                        UrlBandera = $"https://raw.githubusercontent.com/stefangabos/world_countries/master/data/flags/64x64/{x.IsoAlpha2.ToLower()}.png"
                    }));
                }
            });
        }
    }
}
