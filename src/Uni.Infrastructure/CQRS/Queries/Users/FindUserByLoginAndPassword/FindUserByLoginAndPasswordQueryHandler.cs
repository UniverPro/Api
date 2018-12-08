using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqBuilder.EFCore;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Users.FindUserByLoginAndPassword
{
    [UsedImplicitly]
    public class FindUserByLoginAndPasswordQueryHandler : IQueryHandler<FindUserByLoginAndPasswordQuery, User>
    {
        private readonly UniDbContext _dbContext;

        public FindUserByLoginAndPasswordQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> Handle(
            FindUserByLoginAndPasswordQuery query,
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
                    var user = await _dbContext
                        .Users
                        .AsNoTracking()
                        .SingleAsync(specification);

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
