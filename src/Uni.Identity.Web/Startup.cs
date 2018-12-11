using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uni.Common.Extensions;
using Uni.Identity.Web.Extensions.Installers;

namespace Uni.Identity.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .InstallDataProtection(
                    "backoffice_identity",
                    _configuration.GetValue<string>("DataProtectionDirectory")
                )
                .InstallIdentityServer(
                    _configuration.GetSection("IdentityServer"),
                    _configuration.GetConnectionString("IdentityServerConfiguration"),
                    _configuration.GetConnectionString("IdentityServerTokens")
                )
                .InstallApplicationServices()
                .InstallMvc();
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env
            )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.InitializeApplication();

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
