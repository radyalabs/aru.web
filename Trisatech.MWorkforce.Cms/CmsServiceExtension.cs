using Trisatech.MWorkforce.Cms.Interfaces;
using Trisatech.MWorkforce.Cms.Services;

namespace Trisatech.MWorkforce.Cms
{
    public static class CmsServiceExtension
    {
        public static IServiceCollection AddServiceCms(this IServiceCollection services)
        {
            services.AddScoped<ITaskReportService, TaskReportService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddSingleton<ISequencerNumberService, SequencerNumberService>();

            return services;
        }
    }
}
