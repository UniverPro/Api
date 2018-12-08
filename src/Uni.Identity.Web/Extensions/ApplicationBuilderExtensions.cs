using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Uni.Identity.Web.Services.Initialization;

namespace Uni.Identity.Web.Extensions
{
    /// <summary>
    ///     Методы расширения для <see cref="IApplicationBuilder" />.
    /// </summary>
    [PublicAPI]
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        ///     Выполняет инициализацию приложения.
        /// </summary>
        /// <param name="app">Расширяемый экземпляр <see cref="IApplicationBuilder" />.</param>
        /// <returns></returns>
        [NotNull]
        public static IApplicationBuilder InitializeApplication([NotNull] this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializationService = scope
                    .ServiceProvider
                    .GetRequiredService<ApplicationInitializationService>();

                initializationService
                    .InitializeAsync()
                    .GetAwaiter()
                    .GetResult();
            }

            return app;
        }
    }
}