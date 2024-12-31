using Risk.Maui.Views.Base;

namespace Risk.Maui.Views;

public partial class CountriesPage : ContentPageBase
{
    private readonly CountriesViewModel _viewModel;

    public CountriesPage(CountriesViewModel viewModel)
    {
        BindingContext = _viewModel = viewModel;
        InitializeComponent();
    }
}