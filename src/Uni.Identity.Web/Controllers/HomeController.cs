using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Uni.Identity.Web.Controllers
{
    /// <summary>
    ///     Контроллер по-умолчанию.
    /// </summary>
    /// <inheritdoc />
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        ///     Обработка маршрута по-умолчанию.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToAction(
                User.IsAuthenticated()
                    ? nameof(AccountController.Authenticated)
                    : nameof(AccountController.Login),
                "Account");
        }
    }
}