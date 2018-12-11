using System;
using System.Threading.Tasks;
using IdentityServer4.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Uni.Common.Utilities;
using Uni.Identity.Web.ViewModels.Error;

namespace Uni.Identity.Web.Controllers
{
    /// <summary>
    ///     Контроллер для обработки ошибок, возникающих в приложении.
    /// </summary>
    /// <inheritdoc />
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ErrorController([NotNull] IIdentityServerInteractionService interaction)
        {
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
        }

        /// <summary>
        ///     Обработчик исключений и ошибок приложения.
        /// </summary>
        /// <param name="errorId">Идентификатор ошибки IdentityServer.</param>
        /// <returns></returns>
        public async Task<IActionResult> Index(string errorId)
        {
            ErrorViewModel viewModel = null;

            // Получаем информацию об ошибке от IdentityServer.
            var message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                viewModel = new ErrorViewModel(message.Error, message.ErrorDescription, message.RequestId);
            }

            // Если не было ошибок от IdentityServer - пытаемся получить исключение и его сообщение.
            if (viewModel == null)
            {
                var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
                if (feature?.Error != null)
                {
                    var lastExceptionMessage = ExceptionUtilities.GetExceptionMessage(feature.Error);
                    if (!string.IsNullOrEmpty(lastExceptionMessage))
                    {
                        viewModel = new ErrorViewModel(lastExceptionMessage);
                    }
                }
            }

            return View(viewModel);
        }
    }
}
