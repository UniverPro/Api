using System.Text.Encodings.Web;
using System.Text.Unicode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Uni.Identity.Web.Extensions.Installers
{
    /// <summary>
    ///     Методы расширения для настройки ASP.NET Core MVC.
    /// </summary>
    public static class MvcInstaller
    {
        /// <summary>
        ///     Настраивает ASP.NET Core MVC в коллекции сервисов приложения.
        /// </summary>
        /// <param name="services">Экземпляр настраиваемой коллекции сервисов.</param>
        /// <returns></returns>
        public static IServiceCollection InstallMvc(this IServiceCollection services)
        {
            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.Converters.Add(new StringEnumConverter
                    {
                        AllowIntegerValues = true,
                        CamelCaseText = true
                    });
                });

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = false;
            });

            services.Configure<WebEncoderOptions>(options =>
            {
                options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All);
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            return services;
        }
    }
}