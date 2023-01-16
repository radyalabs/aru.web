using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trisatech.MWorkforce.Infrastructure.Interface;
using Trisatech.MWorkforce.Infrastructure.Interfaces;
using Trisatech.MWorkforce.Infrastructure.Services;

namespace Trisatech.MWorkforce.Infrastructure
{
    public static class InfrastructureServiceExtension
    {
        public static IServiceCollection AddGoogleMapApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<GoogleMapsAPI.GoogleMapsAPI, GoogleMapsAPI.GoogleMapsAPIService>(
                factory => new GoogleMapsAPI.GoogleMapsAPIService(configuration.GetValue<string>("ApplicationSetting:GoogleBaseApiUrl"),
                configuration.GetValue<string>("ApplicationSetting:GoogleMapsKey"),
                "json"));

            return services;
        }

        public static IServiceCollection AddAzureStorageAccount(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAzureStorageService>(factory =>
            {
                return new AzureStorageService(
                    configuration.GetValue<string>("AzureBlobSetings:StorageAccount"),
                    configuration.GetValue<string>("AzureBlobSetings:StorageKey"),
                    configuration.GetValue<string>("AzureBlobSetings:ContainerName"));
            });

            return services;
        }

        public static IServiceCollection AddTextReader(this IServiceCollection services)
        {
            services.AddSingleton<ITextFileReader, TextFileReader>();

            return services;
        }
    }
}
