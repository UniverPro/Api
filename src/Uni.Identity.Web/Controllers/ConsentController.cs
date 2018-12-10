using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Uni.Identity.Web.Interfaces;
using Uni.Identity.Web.MVC.Filters.ActionFilters;
using Uni.Identity.Web.ViewModels.Consent;

namespace Uni.Identity.Web.Controllers
{
    /// <summary>
    ///     Контроллер для UI контроля доступа.
    /// </summary>
    /// <inheritdoc />
    [Authorize]
    [SecurityHeaders]
    public class ConsentController : Controller
    {
        private readonly IConsentService _consentService;

        public ConsentController(IConsentService consentService)
        {
            _consentService = consentService ?? throw new ArgumentNullException(nameof(consentService));
        }

        /// <summary>
        ///     Отображает экран контроля доступа.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await _consentService.BuildViewModelAsync(returnUrl);
            if (vm != null) return View(nameof(Index), vm);
            return RedirectToAction(nameof(ErrorController.Index), "Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var result = await _consentService.ProcessConsent(model);
            if (result.IsRedirect) return Redirect(result.RedirectUri);
            if (result.HasValidationError) ModelState.AddModelError("", result.ValidationError);
            if (result.ShowView) return View("Index", result.ViewModel);
            return RedirectToAction(nameof(ErrorController.Index), "Error");
        }
    }
}