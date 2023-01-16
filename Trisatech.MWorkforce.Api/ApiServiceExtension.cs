using Trisatech.MWorkforce.Api.Interfaces;
using Trisatech.MWorkforce.Api.Services;

namespace Trisatech.MWorkforce.Api
{
    public static class ApiServiceExtension
    {
        public static IServiceCollection AddServiceApi(this IServiceCollection services)
        {
            services.AddScoped<IUserReportService, UserReportService>();
            services.AddScoped<IRefService, RefService>();
            services.AddSingleton<ISequencerNumberService, SequencerNumberService>();

            return services;
        }
    }
}
