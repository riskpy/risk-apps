using Risk.Maui.Views.Base;

namespace Risk.Maui.Views;

public partial class MainPage : ContentPageBase
{
    public MainPage(MainViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
