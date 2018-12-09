using System;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Uni.Api.Client;
using Uni.Api.Shared.Responses;
using Uni.Identity.Web.Configuration.Options;
using Uni.Identity.Web.Configuration.Options.IdentityServer;
using Uni.Identity.Web.Interfaces;
using Uni.Identity.Web.ViewModels.Account.LoggedOut;
using Uni.Identity.Web.ViewModels.Account.Login;
using Uni.Identity.Web.ViewModels.Account.Logout;

namespace Uni.Identity.Web.Services
{
    /// <summary>
    ///     Реализация вспомогательного сервиса для контроллера аккаунта пользователя.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IdentityServerCommonOptions _identityServerCommonOptions;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IUniApiClient _uniApiClient;

        public AccountService(
            IOptionsSnapshot<IdentityServerConfiguration> identityServerCommonOptions,
            [NotNull] IHttpContextAccessor contextAccessor,
            [NotNull] IUniApiClient uniApiClient,
            [NotNull] IIdentityServerInteractionService interaction)
        {
            _identityServerCommonOptions = identityServerCommonOptions?.Value?.Common ??
                                           throw new ArgumentNullException(nameof(identityServerCommonOptions));
            _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
            _uniApiClient = uniApiClient ?? throw new ArgumentNullException(nameof(uniApiClient));
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
        }

        public async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);

            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                RememberMe = _identityServerCommonOptions.AllowRememberLogin,
                AllowRememberLogin = _identityServerCommonOptions.AllowRememberLogin
            };
        }

        public async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel loginViewModel)
        {
            var newViewModel = await BuildLoginViewModelAsync(loginViewModel?.ReturnUrl);
            newViewModel.Username = loginViewModel?.Username;
            var rememberMe = loginViewModel?.RememberMe;
            if (rememberMe.HasValue)
            {
                newViewModel.RememberMe = rememberMe.Value;
            }

            return newViewModel;
        }

        public async Task<UserDetailsResponseModel> FindUserAsync(string login, string password)
        {
            if (string.IsNullOrEmpty(login))
            {
                return null;
            }
            
            return await _uniApiClient.FindUserByLoginAndPasswordAsync(login, password);
        }

        public async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId,
                ShowLogoutPrompt = _identityServerCommonOptions.ShowLogoutPrompt
            };

            var user = _contextAccessor.HttpContext.User;
            if (user?.Identity.IsAuthenticated != true)
            {
                // если пользователь не аутентифицирован, то просто показываем страницу об успешном выходе
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // для пользователя установлена настройка об автоматическом выходе, без запроса
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            return vm;
        }

        public async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // Получаем информацию о текущем контексте операции (client name, post logout redirect URI, federated signout iframe)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);

            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = _identityServerCommonOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };

            var httpContext = _contextAccessor.HttpContext;
            var user = httpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                var idp = user.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
                if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
                {
                    var providerSupportsSignOut = await httpContext.GetSchemeSupportsSignOutAsync(idp);
                    if (providerSupportsSignOut)
                    {
                        if (vm.LogoutId == null) vm.LogoutId = await _interaction.CreateLogoutContextAsync();

                        vm.ExternalAuthenticationScheme = idp;
                    }
                }
            }

            return vm;
        }
    }
}