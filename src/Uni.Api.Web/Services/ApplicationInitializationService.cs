using System;
using JetBrains.Annotations;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Uni.Common.Interfaces;
using Uni.DataAccess.Contexts;

namespace Uni.Api.Web.Services
{
    internal sealed class ApplicationInitializationService : IApplicationInitializationService
    {
        private readonly UniDbContext _uniDbContext;
        private readonly IHostingEnvironment _environment;

        public ApplicationInitializationService([NotNull] IHostingEnvironment environment, [NotNull] UniDbContext uniDbContext)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _uniDbContext = uniDbContext ?? throw new ArgumentNullException(nameof(uniDbContext));
        }

        public void Initialize()
        {
            InitializeQueryOptimizer();

            if (!_environment.IsEnvironment("ef"))
            {
                InitializeDatabase();
            }
        }

        private void InitializeDatabase()
        {
            _uniDbContext.Database.Migrate();
        }

        private static void InitializeQueryOptimizer()
        {
            LinqKitExtension.QueryOptimizer = ExpressionOptimizer.visit;
        }
    }
}