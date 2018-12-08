using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Uni.Api.Client
{
    [PublicAPI]
    public static class HttpClientFactoryExtensions
    {
        /// <summary>
        ///     Adds a <see cref="IUniApiClient" /> to the DI container
        /// </summary>
        /// <param name="services">Container</param>
        /// <param name="settings">Optional. Settings to configure the instance with</param>
        /// <returns></returns>
        public static IHttpClientBuilder AddUniApiClient(
            [NotNull] this IServiceCollection services,
            [CanBeNull] RefitSettings settings = null
            )
        {
            return services.AddRefitClient<IUniApiClient>(settings);
        }
    }
}
