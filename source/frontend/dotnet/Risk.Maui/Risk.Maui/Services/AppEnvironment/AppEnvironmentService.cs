using Risk.API.Client.Api;
using Risk.API.Client.Client;
using Risk.Common.Helpers;

namespace Risk.Maui.Services.AppEnvironment
{
    public class AppEnvironmentService : IAppEnvironmentService
    {
        private readonly IAutApi _autApi;
        private readonly IGenApi _genApi;
        private readonly IGloApi _gloApi;
        private readonly IMsjApi _msjApi;
        private readonly IRepApi _repApi;

        public IAutApi AutApi { get; private set; }
        public IGenApi GenApi { get; private set; }
        public IGloApi GloApi { get; private set; }
        public IMsjApi MsjApi { get; private set; }
        public IRepApi RepApi { get; private set; }

        public AppEnvironmentService(Configuration configuration)
        {
            _autApi = new AutApi();
            _genApi = new GenApi();
            _gloApi = new GloApi();
            _msjApi = new MsjApi();
            _repApi = new RepApi();

            UpdateDependencies(configuration);
        }

        public AppEnvironmentService(string basePath, string apiKey, string accessToken)
        {
            _autApi = new AutApi();
            _genApi = new GenApi();
            _gloApi = new GloApi();
            _msjApi = new MsjApi();
            _repApi = new RepApi();

            UpdateDependencies(basePath, apiKey, accessToken);
        }

        public void UpdateDependencies(Configuration configuration)
        {
            _autApi.Configuration = configuration;
            _genApi.Configuration = configuration;
            _gloApi.Configuration = configuration;
            _msjApi.Configuration = configuration;
            _repApi.Configuration = configuration;

            AutApi = _autApi;
            GenApi = _genApi;
            GloApi = _gloApi;
            MsjApi = _msjApi;
            RepApi = _repApi;
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
            Configuration config = (Configuration)_autApi.Configuration;

            // Configure Bearer token for authorization: AccessToken
            config.AccessToken = accessToken;

            UpdateDependencies(config);
        }
    }
}



