using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Uni.Common.Interfaces;

namespace Uni.Common.Extensions
{
    [PublicAPI]
    public static class ApplicationBuilderExtensions
    {
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
                    .GetRequiredService<IApplicationInitializationService>();

                initializationService.Initialize();
            }

            return app;
        }
    }
}