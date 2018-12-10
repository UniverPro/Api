using System;
using System.Data;
using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Uni.Common.Interfaces;
using Uni.Identity.Web.Configuration.Options;

namespace Uni.Identity.Web.Services
{
    internal sealed class ApplicationInitializationService : IApplicationInitializationService
    {
        private readonly IHostingEnvironment _environment;
        private readonly IdentityServerConfiguration _configuration;
        private readonly ConfigurationDbContext _configurationContext;
        private readonly PersistedGrantDbContext _persistedGrantContext;

        public ApplicationInitializationService(
            [NotNull] IHostingEnvironment environment,
            [NotNull] PersistedGrantDbContext persistedGrantContext,
            [NotNull] ConfigurationDbContext configurationContext,
            IOptionsSnapshot<IdentityServerConfiguration> configuration
            )
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _persistedGrantContext =
                persistedGrantContext ?? throw new ArgumentNullException(nameof(persistedGrantContext));
            _configurationContext =
                configurationContext ?? throw new ArgumentNullException(nameof(configurationContext));
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void Initialize()
        {
            if (!_environment.IsEnvironment("ef"))
            {
                InitializeDatabase();
                SynchronizeConfiguration();
            }
        }

        private void InitializeDatabase()
        {
            _persistedGrantContext.Database.Migrate();

            _configurationContext.Database.EnsureDeleted();
            _configurationContext.Database.Migrate();
        }

        private void SynchronizeConfiguration()
        {
            using (var transaction = _configurationContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    SynchronizeClients();
                    SynchronizeApiResources();
                    SynchronizeIdentityResources();
                    _configurationContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void SynchronizeClients()
        {
            var configurationClients = _configuration.ResourcesAndClients.Clients;

            var currentClients = _configurationContext.Clients.ToArray();
            _configurationContext.Clients.RemoveRange(currentClients);

            var newClients = configurationClients.Select(x => x.ToEntity());
            _configurationContext.Clients.AddRange(newClients);
        }

        private void SynchronizeApiResources()
        {
            var configurationApiResources = _configuration.ResourcesAndClients.ApiResources;

            var currentApiResources = _configurationContext.ApiResources.ToArray();
            _configurationContext.ApiResources.RemoveRange(currentApiResources);

            var newApiResources = configurationApiResources.Select(x => x.ToEntity());
            _configurationContext.ApiResources.AddRange(newApiResources);
        }

        private void SynchronizeIdentityResources()
        {
            var configurationIdentityResources = _configuration.ResourcesAndClients.IdentityResources;

            var currentIdentityResources = _configurationContext.IdentityResources.ToArray();
            _configurationContext.IdentityResources.RemoveRange(currentIdentityResources);

            var newIdentityResources = configurationIdentityResources.Select(x => x.ToEntity());
            _configurationContext.IdentityResources.AddRange(newIdentityResources);
        }
    }
}
