namespace Uni.Identity.Web.Configuration.Options.IdentityServer
{
    /// <summary>
    ///     Параметры сертификата для подписи токенов.
    /// </summary>
    public class SigningCertificateOptions
    {
        /// <summary>
        ///     Расположение сертификата.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        ///     Пароль сертификата.
        /// </summary>
        public string Password { get; set; }
    }
}
