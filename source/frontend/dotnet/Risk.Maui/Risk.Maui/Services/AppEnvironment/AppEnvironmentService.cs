using Risk.API.Client.Api;
using Risk.API.Client.Client;
using Risk.Common.Helpers;

namespace Risk.Maui.Services.AppEnvironment
{
    public class AppEnvironmentService : IAppEnvironmentService
    {
        public IAutApi AutApi { get; private set; }
        public IGenApi GenApi { get; private set; }
        public IGloApi GloApi { get; private set; }
        public IMsjApi MsjApi { get; private set; }
        public IRepApi RepApi { get; private set; }

        public AppEnvironmentService(Configuration configuration)
        {
            UpdateDependencies(configuration);
        }

        public AppEnvironmentService(string basePath, string apiKey, string accessToken)
        {
            UpdateDependencies(basePath, apiKey, accessToken);
        }

        public void UpdateDependencies(Configuration configuration)
        {
            AutApi = new AutApi(configuration);
            GenApi = new GenApi(configuration);
            GloApi = new GloApi(configuration);
            MsjApi = new MsjApi(configuration);
            RepApi = new RepApi(configuration);
        }

        public void UpdateDependencies(string basePath, string apiKey, string accessToken)
        {
            Configuration config = new Configuration();

            config.BasePath = basePath;
            // Configure API key authorization: RiskAppKey
            config.AddApiKey(RiskConstants.HEADER_RISK_APP_KEY, apiKey);
            // Configure Bearer token for authorization: AccessToken
            config.AccessToken = accessToken;

            UpdateDependencies(config);
        }

        public void UpdateDependencies(string accessToken)
        {
            Configuration config = (Configuration)AutApi.Configuration;

            // Configure Bearer token for authorization: AccessToken
            config.AccessToken = accessToken;

            UpdateDependencies(config);
        }
    }
}



