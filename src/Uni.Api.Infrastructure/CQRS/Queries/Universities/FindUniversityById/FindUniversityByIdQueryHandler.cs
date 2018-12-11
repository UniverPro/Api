using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Universities.FindUniversityById
{
    [UsedImplicitly]
    public class FindUniversityByIdQueryHandler : IQueryHandler<FindUniversityByIdQuery, University>
    {
        private readonly UniDbContext _dbContext;

        public FindUniversityByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<University> Handle(
            FindUniversityByIdQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var university = await _dbContext
                        .Universities
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.UniversityId, cancellationToken);

                    transaction.Commit();
                    return university;
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
