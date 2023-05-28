namespace Risk.Maui.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        #region Settings keys
        private const string API_BASE_PATH = "API_BASE_PATH";
        private const string API_KEY = "API_KEY";
        private const string ACCESS_TOKEN = "ACCESS_TOKEN";
        private const string REFRESH_TOKEN = "REFRESH_TOKEN";
        private const string DEVICE_TOKEN = "DEVICE_TOKEN";
        private const string IS_USER_LOGGED_IN = "IS_USER_LOGGED_IN";
        private const string IS_CONNECTED = "IS_CONNECTED";
        private const string LATITUDE = "LATITUDE";
        private const string LONGITUDE = "LONGITUDE";
        private const string ALLOW_GPS_LOCATION = "ALLOW_GPS_LOCATION";
        #endregion

        #region Settings default values
        private readonly string API_BASE_PATH_DEFAULT = GlobalSettings.Instance.ApiBasePath;
        private readonly string API_KEY_DEFAULT = GlobalSettings.Instance.ApiKey;
        private readonly string ACCESS_TOKEN_DEFAULT = string.Empty;
        private readonly string REFRESH_TOKEN_DEFAULT = string.Empty;
        private readonly string DEVICE_TOKEN_DEFAULT = string.Empty;
        private readonly bool IS_USER_LOGGED_IN_DEFAULT = false;
        private readonly bool IS_CONNECTED_DEFAULT = true;
        private readonly double LATITUDE_DEFAULT = 0;
        private readonly double LONGITUDE_DEFAULT = 0;
        private readonly bool ALLOW_GPS_LOCATION_DEFAULT = false;
        #endregion

        #region Settings properties
        public string ApiBasePath
        {
            get => Preferences.Get(API_BASE_PATH, API_BASE_PATH_DEFAULT);
            set => Preferences.Set(API_BASE_PATH, value);
        }

        public string ApiKey
        {
            get => Preferences.Get(API_KEY, API_KEY_DEFAULT);
            set => Preferences.Set(API_KEY, value);
        }

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

        public double Latitude
        {
            get => Preferences.Get(LATITUDE, LATITUDE_DEFAULT);
            set => Preferences.Set(LATITUDE, value);
        }

        public double Longitude
        {
            get => Preferences.Get(LONGITUDE, LONGITUDE_DEFAULT);
            set => Preferences.Set(LONGITUDE, value);
        }

        public bool AllowGpsLocation
        {
            get => Preferences.Get(ALLOW_GPS_LOCATION, ALLOW_GPS_LOCATION_DEFAULT);
            set => Preferences.Set(ALLOW_GPS_LOCATION, value);
        }
        #endregion
    }
}
