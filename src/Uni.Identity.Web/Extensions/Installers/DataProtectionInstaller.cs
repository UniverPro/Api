using System;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;

namespace Uni.Identity.Web.Extensions.Installers
{
    public static class DataProtectionInstaller
    {
        public static IServiceCollection InstallDataProtection(
            this IServiceCollection services,
            string applicationDiscriminator,
            string dataProtectionDirectory
            )
        {
            var directory = new DirectoryInfo(dataProtectionDirectory);
            if (!directory.Exists)
            {
                throw new Exception("Директория, используемая для механизма DataProtection не существует!");
            }

            services
                .AddDataProtection(options => options.ApplicationDiscriminator = applicationDiscriminator)
                .PersistKeysToFileSystem(directory);

            return services;
        }
    }
}
