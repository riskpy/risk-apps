namespace Risk.Maui;

public class GlobalSettings
{
    public const string DefaultEndpoint = "http://YOUR_IP_OR_DNS_NAME";
    // http://10.0.2.2:5000
    // http://localhost:5000

    public GlobalSettings()
    {
        ApiBasePath = DefaultEndpoint;
        ApiKey = "INSERT API KEY";
    }

    public static GlobalSettings Instance { get; } = new GlobalSettings();

    public string ApiBasePath { get; set; }

    public string ApiKey { get; set; }
}
