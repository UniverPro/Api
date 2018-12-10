using System;
using System.Globalization;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Uni.Identity.Web.Configuration.Options;
using Uni.Identity.Web.Configuration.Options.IdentityServer;
using Uni.Identity.Web.Interfaces;
using Uni.Identity.Web.MVC.Filters.ActionFilters;
using Uni.Identity.Web.ViewModels.Account.Login;
using Uni.Identity.Web.ViewModels.Account.Logout;

namespace Uni.Identity.Web.Controllers
{
    /// <summary>
    ///     Контроллер аккаунта пользователя.
    /// </summary>
    /// <inheritdoc />
    [Authorize]
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IEventService _events;
        private readonly IdentityServerCommonOptions _identityServerCommonOptions;
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            IAccountService accountService,
            IIdentityServerInteractionService interaction,
            IEventService events,
            IOptionsSnapshot<IdentityServerConfiguration> identityServerCommonOptions
            )
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            _events = events ?? throw new ArgumentNullException(nameof(events));
            _identityServerCommonOptions = identityServerCommonOptions?.Value?.Common ??
                                           throw new ArgumentNullException(nameof(identityServerCommonOptions));
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        /// <summary>
        ///     Запрос на отображение формы для входа.
        /// </summary>
        /// <param name="returnUrl">Ссылка, на которую необходимо вернуть пользователя после успешного входа.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (User.IsAuthenticated())
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var viewModel = await _accountService.BuildLoginViewModelAsync(returnUrl);
            return View(viewModel);
        }

        /// <summary>
        ///     Обработчик формы для входа пользователя.
        /// </summary>
        /// <param name="model">Модель данных для страницы входа пользователя.</param>
        /// <param name="button">Нажатая кнопка на странице входа.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string button)
        {
            if (User.IsAuthenticated())
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (button != "login")
            {
                var context = await _interaction.GetAuthorizationContextAsync(model?.ReturnUrl);
                if (context != null)
                {
                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
                    return Redirect(model?.ReturnUrl);
                }

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (ModelState.IsValid)
            {
                var (user, error) = await _accountService
                    .FindUserAsync(model.Username, model.Password);

                if (error != null)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                    ModelState.AddModelError("", error.Message);
                }
                else if (user != null)
                {
                    var userName = user.Login;
                    var subjectId = user.Id.ToString(CultureInfo.InvariantCulture);
                    await _events.RaiseAsync(
                        new UserLoginSuccessEvent(
                            userName,
                            subjectId,
                            userName
                        )
                    );

                    // Явно устанавливаем окончание жизни кук, только если пользователь выбрал "Запомнить"
                    // в противном случае, срок жизни кук будет зависеть от настроек middleware, отвечающей за куки.
                    AuthenticationProperties props = null;
                    if (_identityServerCommonOptions.AllowRememberLogin && model.RememberMe)
                    {
                        props = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(_identityServerCommonOptions.LoginDuration)
                        };
                    }

                    // выпускаем куку для аутентификации пользователя на основе уникального идентификатора и имени пользователя
                    await HttpContext.SignInAsync(subjectId, userName, props);

                    // необходимо удостовериться, что ссылка, по которой мы хотим перенаправить пользователя всё ещё корректна,
                    // либо является локальной ссылкой внутри приложения
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
                else
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                    ModelState.AddModelError("", "Invalid user data");
                }
            }

            // Если что-то пошло не так - переполучаем модель и рисуем вьюху заново.
            var viewModel = await _accountService.BuildLoginViewModelAsync(model);
            return View(viewModel);
        }

        /// <summary>
        ///     Отображает страницу с информацией о том, что доступ запрещён.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        /// <summary>
        ///     Отображает страницу с информацией о том, что пользователь успешно аутентифицирован.
        /// </summary>
        /// <returns></returns>
        public IActionResult Authenticated()
        {
            return View();
        }

        /// <summary>
        ///     Отображает страницу подтверждения выхода пользователя.
        /// </summary>
        /// <param name="logoutId">Id операции выхода пользователя.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var viewModel = await _accountService.BuildLogoutViewModelAsync(logoutId);

            if (viewModel.ShowLogoutPrompt == false)
            {
                return await Logout(viewModel);
            }

            return View(viewModel);
        }

        /// <summary>
        ///     Обработчик подтверждения выхода пользователя.
        /// </summary>
        /// <param name="model">Модель для экрана подтверждения выхода пользователя.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            // Получаем модель для экрана успешного выхода пользователя, чтобы знать что показать пользователю.
            var viewModel = await _accountService.BuildLoggedOutViewModelAsync(model.LogoutId);

            if (User?.Identity.IsAuthenticated == true)
            {
                // Удаляем аутентификационную куку
                await HttpContext.SignOutAsync();

                // Эммитим событие о выходе пользователя
                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
            }

            return View("LoggedOut", viewModel);
        }
    }
}
