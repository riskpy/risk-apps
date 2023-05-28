using Risk.API.Client.Api;
using Risk.API.Client.Client;

namespace Risk.Maui.Services.AppEnvironment
{
    public interface IAppEnvironmentService
    {
        IAutApi AutApi { get; }
        IGenApi GenApi { get; }
        IGloApi GloApi { get; }
        IMsjApi MsjApi { get; }
        IRepApi RepApi { get; }

        void UpdateDependencies(Configuration configuration);
        void UpdateDependencies(string basePath, string apiKey, string accessToken);
    }
}

