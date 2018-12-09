using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Uni.Identity.Web.Configuration.Options;
using Uni.Identity.Web.Configuration.Options.IdentityServer;
using Uni.Identity.Web.Services;

namespace Uni.Identity.Web.Extensions.Installers
{
    /// <summary>
    ///     Методы расширения для настройки IdentityServer4.
    /// </summary>
    public static class IdentityServerInstaller
    {
        /// <summary>
        ///     Настраивает сервисы IdentityServer4.
        /// </summary>
        /// <param name="services">Экземпляр настраиваемой коллекции сервисов.</param>
        /// <param name="identityServerConfiguration">Срез настроек, содержащий параметры конфигурации IdentityServer4.</param>
        /// <param name="configurationDatabaseConnectionString">
        ///     Строка соединения с базой данных, в которой будет содержаться
        ///     конфигурация IdentityServer4.
        /// </param>
        /// <param name="tokensDatabaseConnectionString">
        ///     Строка соединения с базой данных, в которой будут содержаться выданные
        ///     токены.
        /// </param>
        /// <returns></returns>
        public static IServiceCollection InstallIdentityServer(
            this IServiceCollection services,
            IConfiguration identityServerConfiguration,
            string configurationDatabaseConnectionString,
            string tokensDatabaseConnectionString)
        {
            var config = identityServerConfiguration.Get<IdentityServerConfiguration>();
            var signingCert = config.SigningCertificate.ToCertificate();
            var migrationsAssembly = typeof(IdentityServerInstaller).GetTypeInfo().Assembly.GetName().Name;

            services.Configure<IdentityServerConfiguration>(identityServerConfiguration);
            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.ErrorUrl = "/error/index";
                    options.UserInteraction.ErrorIdParameter = "errorId";
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                })
                .AddSigningCredential(signingCert)
                .AddConfigurationStore(store =>
                {
                    store.ConfigureDbContext = builder =>
                    {
#if DEBUG
                        builder.EnableSensitiveDataLogging();
#endif
                        builder.UseSqlServer(configurationDatabaseConnectionString, sql =>
                        {
                            sql.MigrationsAssembly(migrationsAssembly);
                            sql.MigrationsHistoryTable("Idsrv4_Migrations_ConfigurationStore");
                        });
                    };
                })
                .AddOperationalStore(store =>
                {
                    store.ConfigureDbContext = builder =>
                    {
#if DEBUG
                        builder.EnableSensitiveDataLogging();
#endif
                        builder.UseSqlServer(tokensDatabaseConnectionString, sql =>
                        {
                            sql.MigrationsAssembly(migrationsAssembly);
                            sql.MigrationsHistoryTable("Idsrv4_Migrations_OperationalStore");
                        });
                    };
                })
                .AddProfileService<ProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
            services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, ConfigureCookieOptions>();
            return services;
        }
    }
}