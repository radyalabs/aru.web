using Microsoft.Extensions.DependencyInjection;
using Trisatech.MWorkforce.Business.Interfaces;
using Trisatech.MWorkforce.Business.Services;

namespace Trisatech.MWorkforce.Business
{
    public static class BusinessServiceExtension
    {
        public static IServiceCollection AddServiceApiBusiness(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IContentManagementService, ContentManagementService>();

            return services;
        }

        public static IServiceCollection AddServiceCmsBusiness(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ISurveyService, SurveyService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IContentManagementService, ContentManagementService>();
            services.AddScoped<IAssignmentReportService, AssignmentReportService>();
            services.AddScoped<ITerritoryService, TerritoryService>();

            return services;
        }
    }
}
