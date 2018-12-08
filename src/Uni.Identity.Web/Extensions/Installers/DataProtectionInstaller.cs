using System;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Uni.Identity.Web.Extensions.Installers
{
    /// <summary>
    ///     Методы расширения для настройки DataProtection сервисов.
    /// </summary>
    public static class DataProtectionInstaller
    {
        /// <summary>
        ///     Настраивает DataProtection сервисы приложения.
        /// </summary>
        /// <param name="services">Экземпляр настраиваемой коллекции сервисов.</param>
        /// <param name="applicationDiscriminator">
        ///     Уникальный идентификатор приложения, для отделения его от других приложений,
        ///     использующих те же ключи.
        /// </param>
        /// <param name="dataProtectionDirectory">
        ///     Директория, которая будет использована для хранения ключей, используемых
        ///     механизмом DataProtection.
        /// </param>
        /// <returns></returns>
        public static IServiceCollection InstallDataProtection(
            this IServiceCollection services,
            string applicationDiscriminator,
            string dataProtectionDirectory)
        {
            var directory = new DirectoryInfo(dataProtectionDirectory);
            if (!directory.Exists)
                throw new Exception("Директория, используемая для механизма DataProtection не существует!");
            services
                .AddDataProtection(options => options.ApplicationDiscriminator = applicationDiscriminator)
                .PersistKeysToFileSystem(directory);
            return services;
        }
    }
}