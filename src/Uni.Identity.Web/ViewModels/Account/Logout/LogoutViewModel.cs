namespace Uni.Identity.Web.ViewModels.Account.Logout
{
    /// <summary>
    ///     Модель данных для экрана выхода пользователя.
    /// </summary>
    public class LogoutViewModel : LogoutInputModel
    {
        /// <summary>
        ///     Отображать ли запрос перед выходом пользователя.
        /// </summary>
        public bool ShowLogoutPrompt { get; set; }
    }
}