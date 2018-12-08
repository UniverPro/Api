using System;

namespace Uni.Identity.Web.Configuration.Options.IdentityServer
{
    /// <summary>
    ///     Общие параметры IdentityServer.
    /// </summary>
    public class IdentityServerCommonOptions
    {
        /// <summary>
        ///     Показывать ли запрос при выходе из сервиса.
        /// </summary>
        public bool ShowLogoutPrompt { get; set; }

        /// <summary>
        ///     Нужно ли автоматически перенаправлять пользователя после выхода из системы по указанной ссылке.
        /// </summary>
        public bool AutomaticRedirectAfterSignOut { get; set; }

        /// <summary>
        ///     Показывать ли пользователю при логине галочку "Запоминать".
        /// </summary>
        public bool AllowRememberLogin { get; set; }

        /// <summary>
        ///     Срок жизни сессии на IdentityServer.
        /// </summary>
        public TimeSpan LoginDuration { get; set; }

        /// <summary>
        ///     Разрешить ли клиентам выдавать токены с оффлайн-доступом к ресурсам.
        /// </summary>
        public bool EnableOfflineAccess { get; set; }

        /// <summary>
        ///     Наименование scope, отвечающего за оффлайн-доступ.
        /// </summary>
        public string OfflineAccessDisplayName { get; set; }

        /// <summary>
        ///     Описание scope, отвечающего за оффлайн-доступ.
        /// </summary>
        public string OfflineAccessDescription { get; set; }

        /// <summary>
        ///     Сообщение о необходимости выбора хотя бы одного разрешения.
        /// </summary>
        public string MustChooseOneErrorMessage { get; set; }

        /// <summary>
        ///     Сообщение о некорректном выборе ответа.
        /// </summary>
        public string InvalidSelectionErrorMessage { get; set; }
    }
}