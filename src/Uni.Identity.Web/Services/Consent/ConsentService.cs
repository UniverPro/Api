using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Uni.Identity.Web.Configuration.Options;
using Uni.Identity.Web.Configuration.Options.IdentityServer;
using Uni.Identity.Web.ViewModels.Consent;

namespace Uni.Identity.Web.Services.Consent
{
    /// <summary>
    ///     Реализация вспомогательного сервиса для контроллера контроля доступа.
    /// </summary>
    public class ConsentService : IConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IdentityServerCommonOptions _identityServerCommonOptions;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ConsentService> _logger;
        private readonly IResourceStore _resourceStore;

        public ConsentService(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            ILogger<ConsentService> logger,
            IOptionsSnapshot<IdentityServerConfiguration> identityServerCommonOptions)
        {
            _interaction = interaction ?? throw new ArgumentNullException(nameof(interaction));
            _clientStore = clientStore ?? throw new ArgumentNullException(nameof(clientStore));
            _resourceStore = resourceStore ?? throw new ArgumentNullException(nameof(resourceStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _identityServerCommonOptions = identityServerCommonOptions?.Value?.Common ??
                                           throw new ArgumentNullException(nameof(identityServerCommonOptions));
        }

        public async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                        return CreateConsentViewModel(model, returnUrl, client, resources);

                    _logger.LogError("No scopes matching: {0}",
                        request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
                }
                else
                {
                    _logger.LogError("Invalid client id: {0}", request.ClientId);
                }
            }
            else
            {
                _logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }

        public async Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model)
        {
            var result = new ProcessConsentResult();

            ConsentResponse grantedConsent = null;

            // user clicked 'no' - send back the standard 'access_denied' response
            if (model.Button == "no")
                grantedConsent = ConsentResponse.Denied;
            // user clicked 'yes' - validate the data
            else if (model.Button == "yes")
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    var scopes = model.ScopesConsented;
                    if (_identityServerCommonOptions.EnableOfflineAccess == false)
                        scopes = scopes.Where(x =>
                            x != IdentityServerConstants.StandardScopes.OfflineAccess);

                    grantedConsent = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = scopes.ToArray()
                    };
                }
                else
                {
                    result.ValidationError = _identityServerCommonOptions.MustChooseOneErrorMessage;
                }
            else
                result.ValidationError = _identityServerCommonOptions.InvalidSelectionErrorMessage;

            if (grantedConsent != null)
            {
                // validate return url is still valid
                var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
                if (request == null) return result;

                // communicate outcome of consent back to IdentityServer
                await _interaction.GrantConsentAsync(request, grantedConsent);

                // indicate that's it ok to redirect back to authorization endpoint
                result.RedirectUri = model.ReturnUrl;
            }
            else
            {
                // we need to redisplay the consent UI
                result.ViewModel = await BuildViewModelAsync(model.ReturnUrl, model);
            }

            return result;
        }

        private ConsentViewModel CreateConsentViewModel(
            ConsentInputModel model,
            string returnUrl,
            Client client,
            Resources resources)
        {
            var vm = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),
                ReturnUrl = returnUrl,
                ClientName = client.ClientName ?? client.ClientId,
                ClientUrl = client.ClientUri,
                ClientLogoUrl = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent
            };

            vm.IdentityScopes = resources
                .IdentityResources
                .Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null))
                .ToArray();

            vm.ResourceScopes = resources
                .ApiResources
                .SelectMany(x => x.Scopes)
                .Select(x => CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null))
                .ToArray();

            if (_identityServerCommonOptions.EnableOfflineAccess && resources.OfflineAccess)
                vm.ResourceScopes = vm.ResourceScopes.Union(new[]
                {
                    GetOfflineAccessScope(
                        vm.ScopesConsented.Contains(
                            IdentityServerConstants.StandardScopes.OfflineAccess)
                        || model == null)
                });

            return vm;
        }

        private static ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        private static ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = _identityServerCommonOptions.OfflineAccessDisplayName,
                Description = _identityServerCommonOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }
    }
}