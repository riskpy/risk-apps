using CommunityToolkit.Maui.Views;

namespace Risk.Maui.Views;

public partial class SpinnerPopup : Popup
{
    public string Message { get; set; }

    public SpinnerPopup(string message)
    {
        Message = message;
        InitializeComponent();
    }
}