using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Uni.Identity.Web.Configuration.Options.IdentityServer;

namespace Uni.Identity.Web.Extensions
{
    public static class SigningCertificateOptionsExtensions
    {
        /// <summary>
        ///     Конвертирует данный экземпляр <see cref="SigningCertificateOptions" /> в <see cref="X509Certificate2" />.
        /// </summary>
        /// <returns></returns>
        [NotNull]
        public static X509Certificate2 ToCertificate([NotNull] this SigningCertificateOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var certFileInfo = new FileInfo(options.Location);
            
            var cert = new X509Certificate2(
                certFileInfo.FullName,
                options.Password,
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet
            );

            return cert;
        }
    }
}
