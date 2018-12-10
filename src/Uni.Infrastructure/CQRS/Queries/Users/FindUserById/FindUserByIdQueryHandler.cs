using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Users.FindUserById
{
    [UsedImplicitly]
    public class FindUserByIdQueryHandler : IQueryHandler<FindUserByIdQuery, User>
    {
        private readonly UniDbContext _dbContext;

        public FindUserByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> Handle(
            FindUserByIdQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var user = await _dbContext
                        .Users
                        .IncludeDefault()
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.UserId, cancellationToken);

                    transaction.Commit();
                    return user;
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
