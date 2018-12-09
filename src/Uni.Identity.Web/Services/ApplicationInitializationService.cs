using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Uni.Identity.Web.Configuration.Options;

namespace Uni.Identity.Web.Services
{
    public class ApplicationInitializationService
    {
        private readonly IdentityServerConfiguration _configuration;
        private readonly ConfigurationDbContext _configurationContext;
        private readonly PersistedGrantDbContext _persistedGrantContext;

        public ApplicationInitializationService(
            [NotNull] PersistedGrantDbContext persistedGrantContext,
            [NotNull] ConfigurationDbContext configurationContext,
            IOptionsSnapshot<IdentityServerConfiguration> configuration)
        {
            _persistedGrantContext = persistedGrantContext ?? throw new ArgumentNullException(nameof(persistedGrantContext));
            _configurationContext = configurationContext ?? throw new ArgumentNullException(nameof(configurationContext));
            _configuration = configuration?.Value ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        ///     Асинхронно выполняет инициализацию приложения.
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            await InitializeDatabaseAsync();
            await SynchronizeConfigurationAsync();
        }

        /// <summary>
        ///     Асинхронно выполняет инициализацию базы данных IdentityServer4.
        /// </summary>
        /// <returns></returns>
        private async Task InitializeDatabaseAsync()
        {
            await _persistedGrantContext.Database.MigrateAsync();
            // Пересоздаёт базу при запуске.
            await _configurationContext.Database.EnsureDeletedAsync();
            await _configurationContext.Database.MigrateAsync();
        }

        /// <summary>
        ///     Асинхронно обновляет конфигурацию IdentityServer4.
        /// </summary>
        /// <returns></returns>
        private async Task SynchronizeConfigurationAsync()
        {
            using (var transaction = await _configurationContext
                .Database
                .BeginTransactionAsync(IsolationLevel.ReadCommitted, CancellationToken.None))
            {
                try
                {
                    await SynchronizeClientsAsync();
                    await SynchronizeApiResourcesAsync();
                    await SynchronizeIdentityResourcesAsync();
                    await _configurationContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        ///     Асинхронно синхронизирует всех клиентов в соответствии с параметрами конфигурации.
        /// </summary>
        /// <returns></returns>
        private async Task SynchronizeClientsAsync()
        {
            var configurationClients = _configuration.ResourcesAndClients.Clients;

            var currentClients = await _configurationContext.Clients.ToArrayAsync();
            _configurationContext.Clients.RemoveRange(currentClients);

            var newClients = configurationClients.Select(x => x.ToEntity());
            await _configurationContext.Clients.AddRangeAsync(newClients);
        }

        /// <summary>
        ///     Асинхронно синхронизирует все ApiResource в соответствии с параметрами конфигурации.
        /// </summary>
        /// <returns></returns>
        private async Task SynchronizeApiResourcesAsync()
        {
            var configurationApiResources = _configuration.ResourcesAndClients.ApiResources;

            var currentApiResources = await _configurationContext.ApiResources.ToArrayAsync();
            _configurationContext.ApiResources.RemoveRange(currentApiResources);

            var newApiResources = configurationApiResources.Select(x => x.ToEntity());
            await _configurationContext.ApiResources.AddRangeAsync(newApiResources);
        }

        /// <summary>
        ///     Асинхронно синхронизирует все IdentityResource в соответствии с параметрами конфигурации.
        /// </summary>
        /// <returns></returns>
        private async Task SynchronizeIdentityResourcesAsync()
        {
            var configurationIdentityResources = _configuration.ResourcesAndClients.IdentityResources;

            var currentIdentityResources = await _configurationContext.IdentityResources.ToArrayAsync();
            _configurationContext.IdentityResources.RemoveRange(currentIdentityResources);

            var newIdentityResources = configurationIdentityResources.Select(x => x.ToEntity());
            await _configurationContext.IdentityResources.AddRangeAsync(newIdentityResources);
        }
    }
}