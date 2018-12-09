using Microsoft.Extensions.DependencyInjection;
using Uni.Identity.Web.Services;

namespace Uni.Identity.Web.Extensions.Installers
{
    /// <summary>
    ///     Методы расширения для настройки сервисов, выполняющих инициализацию приложения.
    /// </summary>
    public static class InitializationServicesInstaller
    {
        /// <summary>
        ///     Настраивает сервисы инициализации приложения.
        /// </summary>
        /// <param name="services">Экземпляр настраиваемой коллекции сервисов.</param>
        /// <returns></returns>
        public static IServiceCollection InstallInitializationServices(
            this IServiceCollection services)
        {
            services.AddScoped<ApplicationInitializationService>();
            return services;
        }
    }
}