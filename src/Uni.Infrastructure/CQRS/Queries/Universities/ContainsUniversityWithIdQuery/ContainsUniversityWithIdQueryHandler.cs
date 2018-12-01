using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.ContainsUniversityWithIdQuery
{
    [UsedImplicitly]
    public class ContainsUniversityWithIdQueryHandler : IQueryHandler<ContainsUniversityWithIdQuery, bool>
    {
        private readonly UniDbContext _dbContext;

        public ContainsUniversityWithIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> Handle(
            ContainsUniversityWithIdQuery query,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var contains = await _dbContext
                        .Universities
                        .AsNoTracking()
                        .AnyAsync(x => x.Id == query.UniversityId, cancellationToken);
                    
                    return contains;
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
