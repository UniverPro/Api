using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Teachers.CheckTeacherExists
{
    [UsedImplicitly]
    public class CheckTeacherExistsQueryHandler : IQueryHandler<CheckTeacherExistsQuery, bool>
    {
        private readonly UniDbContext _dbContext;

        public CheckTeacherExistsQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> Handle(
            CheckTeacherExistsQuery query,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var teacher = await _dbContext
                        .Teachers
                        .AsNoTracking()
                        .AnyAsync(x => x.Id == query.TeacherId, cancellationToken);
                    
                    transaction.Commit();
                    return teacher;
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
