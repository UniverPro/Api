using Microsoft.Extensions.DependencyInjection;
using Uni.Identity.Web.Services;
using Uni.Identity.Web.Services.Account;
using Uni.Identity.Web.Services.Consent;

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
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}