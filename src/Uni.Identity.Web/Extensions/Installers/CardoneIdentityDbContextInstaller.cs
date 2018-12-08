using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Uni.DataAccess.Contexts;

namespace Uni.Identity.Web.Extensions.Installers
{
    public static class CardoneIdentityDbContextInstaller
    {
        /// <summary>
        ///     Настраивает <see cref="UniDbContext" /> в коллекции сервисов приложения.
        /// </summary>
        /// <param name="services">Экземпляр настраиваемой коллекции сервисов.</param>
        /// <param name="connectionString">Строка соединения с базой данных.</param>
        /// <returns></returns>
        public static IServiceCollection InstallDbContext(
            this IServiceCollection services,
            string connectionString)
        {
            var migrationsAssembly = typeof(UniDbContext).GetTypeInfo().Assembly.FullName;

            services.AddDbContext<UniDbContext>(options =>
            {
#if DEBUG
                options.EnableSensitiveDataLogging();
#endif
                options.UseSqlServer(connectionString, sql =>
                {
                    sql.CommandTimeout(600);
                    sql.MigrationsAssembly(migrationsAssembly);
                });
            });
            return services;
        }
    }
}