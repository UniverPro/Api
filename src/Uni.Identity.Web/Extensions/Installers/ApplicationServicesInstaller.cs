using System;
using Microsoft.Extensions.DependencyInjection;
using Uni.Api.Client;
using Uni.Identity.Web.Interfaces;
using Uni.Identity.Web.Services;

namespace Uni.Identity.Web.Extensions.Installers
{
    /// <summary>
    ///     Методы расширения для настройки сервисов уровня приложения.
    /// </summary>
    public static class ApplicationServicesInstaller
    {
        /// <summary>
        ///     Настраивает сервисы уровня приложения.
        /// </summary>
        /// <param name="services">Экземпляр настраиваемой коллекции сервисов.</param>
        /// <returns></returns>
        public static IServiceCollection InstallApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConsentService, ConsentService>();
            services.AddUniApiClient()
                .ConfigureHttpClient(x=>
                    {
                        x.BaseAddress = new Uri("http://localhost:5001/api/v1");
                    }
                );
            return services;
        }
    }
}