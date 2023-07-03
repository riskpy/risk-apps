namespace Risk.Maui.Views;

public partial class SamplePage : ContentPage
{
    public SamplePage(SampleViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}
