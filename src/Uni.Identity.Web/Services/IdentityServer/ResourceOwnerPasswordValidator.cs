using System;
using System.Globalization;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;

namespace Uni.Identity.Web.Services.IdentityServer
{
    /// <summary>
    ///     Сервис, используемый IdentityServer-ом при аутентификации по логину и паролю (Resource Owner Credentials Flow).
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IEventService _events;
        private readonly ILogger<ResourceOwnerPasswordValidator> _logger;
        private readonly IUserService _userService;

        public ResourceOwnerPasswordValidator(
            ILogger<ResourceOwnerPasswordValidator> logger,
            IEventService events,
            IUserService userService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _events = events ?? throw new ArgumentNullException(nameof(events));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userService.FindAsync(context.UserName, context.Password);
            if (user != null)
            {
                var subjectId = user.Id.ToString(CultureInfo.InvariantCulture);
                _logger.LogInformation($"Учётные данные подтверждены для пользователя: {context.UserName}");
                await _events.RaiseAsync(
                    new UserLoginSuccessEvent(context.UserName, subjectId, context.UserName, false));
                context.Result = new GrantValidationResult(subjectId, OidcConstants.AuthenticationMethods.Password);
                return;
            }

            _logger.LogInformation($"Не удалось найти пользователя с указанным именем: {context.UserName}");
            await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", false));
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}