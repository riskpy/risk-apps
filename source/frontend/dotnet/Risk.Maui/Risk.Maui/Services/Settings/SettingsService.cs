namespace Risk.Maui.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        #region Settings keys
        private const string ACCESS_TOKEN = "ACCESS_TOKEN";
        private const string REFRESH_TOKEN = "REFRESH_TOKEN";
        private const string DEVICE_TOKEN = "DEVICE_TOKEN";
        private const string IS_USER_LOGGED_IN = "IS_USER_LOGGED_IN";
        private const string IS_CONNECTED = "IS_CONNECTED";
        #endregion

        #region Settings default values
        private readonly string ACCESS_TOKEN_DEFAULT = string.Empty;
        private readonly string REFRESH_TOKEN_DEFAULT = string.Empty;
        private readonly string DEVICE_TOKEN_DEFAULT = string.Empty;
        private readonly bool IS_USER_LOGGED_IN_DEFAULT = false;
        private readonly bool IS_CONNECTED_DEFAULT = true;
        #endregion

        #region Settings properties
        public string AccessToken
        {
            get => Preferences.Get(ACCESS_TOKEN, ACCESS_TOKEN_DEFAULT);
            set => Preferences.Set(ACCESS_TOKEN, value);
        }

        public string RefreshToken
        {
            get => Preferences.Get(REFRESH_TOKEN, REFRESH_TOKEN_DEFAULT);
            set => Preferences.Set(REFRESH_TOKEN, value);
        }

        public string DeviceToken
        {
            get => Preferences.Get(DEVICE_TOKEN, DEVICE_TOKEN_DEFAULT);
            set => Preferences.Set(DEVICE_TOKEN, value);
        }

        public bool IsUserLoggedIn
        {
            get => Preferences.Get(IS_USER_LOGGED_IN, IS_USER_LOGGED_IN_DEFAULT);
            set => Preferences.Set(IS_USER_LOGGED_IN, value);
        }

        public bool IsConnected
        {
            get => Preferences.Get(IS_CONNECTED, IS_CONNECTED_DEFAULT);
            set => Preferences.Set(IS_CONNECTED, value);
        }
        #endregion
    }
}
