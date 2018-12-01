using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.FindUniversities
{
    [UsedImplicitly]
    public class FindUniversitiesQueryHandler : IQueryHandler<FindUniversitiesQuery, IEnumerable<University>>
    {
        private readonly UniDbContext _dbContext;

        public FindUniversitiesQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<University>> Handle(
            FindUniversitiesQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();

            var specification = query.ToSpecification();

            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var universities = await _dbContext
                        .Universities
                        .AsNoTracking()
                        .ExeSpec(specification)
                        .ToListAsync(cancellationToken);

                    transaction.Commit();
                    return universities;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
