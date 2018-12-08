using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Uni.Identity.Web.Services.Authentication.Cookies
{
    /// <summary>
    ///     Class for setting cookie authentication parameters
    ///     https://github.com/IdentityServer/IdentityServer4/issues/1783#issuecomment-366357962
    /// </summary>
    public class ConfigureCookieOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        public void Configure(CookieAuthenticationOptions options)
        {
        }

        public void Configure(string name, CookieAuthenticationOptions options)
        {
            // https://github.com/IdentityServer/IdentityServer4/blob/2.2.0/src/IdentityServer4/Configuration/DependencyInjection/ConfigureInternalCookieOptions.cs#L23
            if (name == IdentityServerConstants.DefaultCookieAuthenticationScheme)
            {
                options.AccessDeniedPath = new PathString("/account/accessDenied");
            }
        }
    }
}