using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using MicroElements.Swashbuckle.FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.CQRS.Commands;
using Uni.Infrastructure.CQRS.Queries;
using Uni.Infrastructure.Interfaces.Services;
using Uni.Infrastructure.Services;
using Uni.WebApi.Configurations;
using Uni.WebApi.Configurations.Filters;

namespace Uni.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                    options => { options.UseCentralRoutePrefix(new RouteAttribute("api/v{version:apiVersion}")); }
                )
                .AddFluentValidation(
                    configuration => AssemblyScanner.FindValidatorsInAssemblyContaining<Startup>().ForEach(
                        pair =>
                        {
                            services.AddTransient(pair.InterfaceType, pair.ValidatorType);
                            services.AddTransient(pair.ValidatorType);
                        }
                    )
                )
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper();

            services.AddMediatR(
                typeof(QueriesMarker),
                typeof(CommandsMarker)
            );
            
            services.AddTransient<IBlobStorageUploader, AzureBlobStorageUploader>();
            services.AddTransient<ErrorHandlingMiddleware>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IPasswordValidator, PasswordHasher>();
            services.Configure<PasswordHasherOptions>(_configuration.GetSection("PasswordHasherOptions"));
            services.AddScoped(resolver =>
            {
                var snapshot = resolver.GetRequiredService<IOptionsSnapshot<PasswordHasherOptions>>();
                return snapshot.Value;
            });

            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<UniDbContext>(
                x =>
                {
                    x.UseSqlServer(
                        _configuration.GetConnectionString("UniDbConnection"),
                        sql => sql.MigrationsAssembly(typeof(UniDbContext).Assembly.FullName)
                    );
                }
            );

            // Automatically perform database migration
            services.BuildServiceProvider().GetService<UniDbContext>().Database.Migrate();

            // Configure versions 
            services.AddApiVersioning(
                o =>
                {
                    o.AssumeDefaultVersionWhenUnspecified = true;
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                }
            );

            // Configure swagger
            services.AddSwaggerGen(
                options =>
                {
                    // Specify api versions
                    options.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "Uni API",
                            Version = "v1"
                        }
                    );

                    options.DescribeAllEnumsAsStrings();
                    options.DescribeStringEnumsInCamelCase();
                    options.AddFluentValidationRules();

                    var xmlPath = Path.Combine(AppContext.BaseDirectory, "Uni.WebApi.xml");
                    options.IncludeXmlComments(xmlPath);

                    options.DescribeAllEnumsAsStrings();

                    options.OperationFilter<RemoveVersionFromParameter>();

                    options.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                    options.DocInclusionPredicate(
                        (version, desc) =>
                        {
                            if (!desc.TryGetMethodInfo(out var methodInfo))
                            {
                                return false;
                            }

                            var versions = methodInfo.DeclaringType
                                .GetCustomAttributes<ApiVersionAttribute>(true)
                                .SelectMany(attr => attr.Versions);

                            return versions.Any(x => $"v{x}" == version);
                        }
                    );
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseScopedSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Uni API v1"));

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
