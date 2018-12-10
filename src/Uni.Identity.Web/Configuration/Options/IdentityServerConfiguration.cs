using JetBrains.Annotations;
using Uni.Identity.Web.Configuration.Options.IdentityServer;

namespace Uni.Identity.Web.Configuration.Options
{
    /// <summary>
    ///     Параметры конфигурации IdentityServer.
    /// </summary>
    [PublicAPI]
    public class IdentityServerConfiguration
    {
        /// <summary>
        ///     Общие параметры IdentityServer.
        /// </summary>
        public IdentityServerCommonOptions Common { get; set; }

        /// <summary>
        ///     Параметры клиентов и ресурсов IdentityServer.
        /// </summary>
        public ResourcesAndClientsOptions ResourcesAndClients { get; set; }

        /// <summary>
        ///     Параметры сертификата для подписи токенов.
        /// </summary>
        public SigningCertificateOptions SigningCertificate { get; set; }
    }
}