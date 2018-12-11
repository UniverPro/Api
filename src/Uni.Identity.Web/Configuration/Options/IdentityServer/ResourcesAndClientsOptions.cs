using IdentityServer4.Models;
using JetBrains.Annotations;

namespace Uni.Identity.Web.Configuration.Options.IdentityServer
{
    /// <summary>
    ///     Параметры клиентов и ресурсов IdentityServer.
    /// </summary>
    [PublicAPI]
    public class ResourcesAndClientsOptions
    {
        /// <summary>
        ///     ApiResource-ы, доступные IdentityServer.
        /// </summary>
        public ApiResource[] ApiResources { get; set; }

        /// <summary>
        ///     IdentityResource-ы, доступные IdentityServer.
        /// </summary>
        public IdentityResource[] IdentityResources { get; set; }

        /// <summary>
        ///     Клиентские приложения IdentityServer-а.
        /// </summary>
        public Client[] Clients { get; set; }
    }
}
