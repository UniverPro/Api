using System;
using Microsoft.Extensions.DependencyInjection;
using Uni.Api.Client;
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

            services.AddUniApiClient()
                .ConfigureHttpClient(
                    x => x.BaseAddress = new Uri("http://localhost:5001/api/v1")
                );

            return services;
        }
    }
}
