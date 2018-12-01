using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Groups.FindGroupById
{
    [UsedImplicitly]
    public class FindGroupByIdQueryHandler : IQueryHandler<FindGroupByIdQuery, Group>
    {
        private readonly UniDbContext _dbContext;

        public FindGroupByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Group> Handle(
            FindGroupByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var group = await _dbContext
                        .Groups
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.GroupId, cancellationToken);
                    
                    return group;
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
