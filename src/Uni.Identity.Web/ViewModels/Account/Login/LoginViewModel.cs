using System.ComponentModel.DataAnnotations;

namespace Uni.Identity.Web.ViewModels.Account.Login
{
    /// <summary>
    ///     Модель данных для страницы входа пользователя.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        ///     Логин пользователя.
        /// </summary>
        [Display(Name = "Логин")]
        [Required(ErrorMessage = "Не указано имя пользователя.")]
        [MaxLength(100, ErrorMessage = "Превышена максимально-допустимая длина.")]
        public string Username { get; set; }

        /// <summary>
        ///     Пароль.
        /// </summary>
        [Display(Name = "Пароль")]
        [Required(ErrorMessage = "Не указан пароль.")]
        [DataType(DataType.Password)]
        [MaxLength(100, ErrorMessage = "Превышена максимально-допустимая длина.")]
        public string Password { get; set; }

        /// <summary>
        ///     Ссылка, на которую необходимо вернуть пользователя после успешного входа.
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Превышена максимально-допустимая длина.")]
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     Флаг. Нужно ли запоминать пользователя.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        ///     Флаг. Разрешено ли запоминать пользователя.
        /// </summary>
        public bool AllowRememberLogin { get; set; }
    }
}