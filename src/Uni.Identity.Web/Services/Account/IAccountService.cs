using System.Threading.Tasks;
using Uni.Api.Shared.Responses;
using Uni.Identity.Web.ViewModels.Account.LoggedOut;
using Uni.Identity.Web.ViewModels.Account.Login;
using Uni.Identity.Web.ViewModels.Account.Logout;

namespace Uni.Identity.Web.Services.Account
{
    /// <summary>
    ///     Вспомогательный сервис для контроллера аккаунта пользователя.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        ///     Создаёт модель данных для страницы входа пользователя.
        /// </summary>
        /// <param name="returnUrl">Ссылка, на которую необходимо вернуть пользователя после успешного входа.</param>
        /// <returns></returns>
        Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl);

        /// <summary>
        ///     Создаёт модель данных для страницы входа пользователя на основе существующей модели.
        /// </summary>
        /// <param name="loginViewModel">Существующая модель данных для страницы входа пользователя.</param>
        /// <returns></returns>
        Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel loginViewModel);

        /// <summary>
        ///     Ищет пользователя по логину и паролю. Если пользователь найден - вернёт экземпляр <see cref="UserResponseModel" />, если нет -
        ///     <see langword="null" />.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns></returns>
        Task<UserResponseModel> FindUserAsync(string login, string password);

        /// <summary>
        ///     Создаёт модель данных для экрана выхода пользователя.
        /// </summary>
        /// <param name="logoutId">Id операции выхода пользователя.</param>
        /// <returns></returns>
        Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId);

        /// <summary>
        ///     Создаёт модель данных для экрана успешного выхода пользователя.
        /// </summary>
        /// <param name="logoutId">Id операции выхода пользователя.</param>
        /// <returns></returns>
        Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId);
    }
}