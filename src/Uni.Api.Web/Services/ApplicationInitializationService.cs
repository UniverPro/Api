using System;
using System.Data;
using System.Linq;
using JetBrains.Annotations;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Web.Configurations.Authorization;
using Uni.Common.Interfaces;

namespace Uni.Api.Web.Services
{
    internal sealed class ApplicationInitializationService : IApplicationInitializationService
    {
        private readonly IHostingEnvironment _environment;
        private readonly UniDbContext _uniDbContext;
        private readonly ILogger<ApplicationInitializationService> _logger;

        public ApplicationInitializationService(
            [NotNull] IHostingEnvironment environment,
            [NotNull] UniDbContext uniDbContext,
            [NotNull] ILogger<ApplicationInitializationService> logger
            )
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Initialize()
        {
            InitializeQueryOptimizer();

            if (!_environment.IsEnvironment("ef"))
            {
                InitializeDatabase();
                SynchronizePermissions();
            }
        }

        private static void InitializeQueryOptimizer()
        {
            LinqKitExtension.QueryOptimizer = ExpressionOptimizer.visit;
        }

        private void SynchronizePermissions()
        {
            using (var transaction = _uniDbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                try
                {
                    var permissions = AuthorizationConfiguration.GetPermissions(typeof(Permissions));

                    var currentPermissions = _uniDbContext.Permissions.ToHashSet();

                    var newPermissions = permissions
                        .Where(
                            x => !currentPermissions.Any(
                                y => string.Equals(x, y.Name, StringComparison.OrdinalIgnoreCase)
                            )
                        ).Select(x => new Permission {Name = x}).ToArray();
                    
                    if (newPermissions.Length != 0)
                    {
                        _logger.LogInformation($"Found {newPermissions.Length} new permissions.");
                        _uniDbContext.Permissions.AddRange(newPermissions);
                        _uniDbContext.SaveChanges();
                    }
                    else
                    {
                        _logger.LogInformation("No permissions were added. The database is already up to date.");
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void InitializeDatabase()
        {
            _uniDbContext.Database.Migrate();
        }
    }
}
