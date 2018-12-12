using Microsoft.Extensions.DependencyInjection;
using Uni.Common.Interfaces;
using Uni.Identity.Web.Interfaces;
using Uni.Identity.Web.Services;

namespace Uni.Identity.Web.Extensions.Installers
{
    internal static class ApplicationServicesInstaller
    {
        public static IServiceCollection InstallApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConsentService, ConsentService>();
            services.AddScoped<IApplicationInitializationService, ApplicationInitializationService>();

            return services;
        }
    }
}
