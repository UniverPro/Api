using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Uni.Identity.Web.Extensions;
using Uni.Identity.Web.Extensions.Installers;

namespace Uni.Identity.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public Startup(
            IConfiguration configuration,
            IHostingEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .InstallDataProtection(
                    "backoffice_identity",
                    _configuration.GetValue<string>("DataProtectionDirectory"))
                .InstallIdentityServer(
                    _configuration.GetSection("IdentityServer"),
                    _configuration.GetConnectionString("IdentityServerConfiguration"),
                    _configuration.GetConnectionString("IdentityServerTokens"))
                .InstallInitializationServices()
                .InstallApplicationServices()
                .InstallMvc();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            if (!_environment.IsEnvironment("ef"))
            {
                app.InitializeApplication();
            }

            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}