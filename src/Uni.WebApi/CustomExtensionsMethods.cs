using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uni.DataAccess;
using Uni.DataAccess.Data;

namespace Uni.WebApi
{
    internal static class CustomExtensionsMethods
    {
        public static void AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<UniDbContext>(x =>
            {
                x.UseSqlServer(
                    configuration.GetConnectionString("UniDbConnection"),
                    sql => sql.MigrationsAssembly(typeof(EfMarker).GetTypeInfo().Assembly.FullName)
                );
            });
        }
    }
}