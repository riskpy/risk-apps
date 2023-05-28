namespace Risk.Maui.Services.Settings
{
    public interface ISettingsService
    {
        string ApiBasePath { get; set; }
        string ApiKey { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        string DeviceToken { get; set; }
        bool IsUserLoggedIn { get; set; }
        bool IsConnected { get; set; }
    }
}
