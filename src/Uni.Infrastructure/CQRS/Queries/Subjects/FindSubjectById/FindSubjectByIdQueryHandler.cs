using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjectById
{
    [UsedImplicitly]
    public class FindSubjectByIdQueryHandler : IQueryHandler<FindSubjectByIdQuery, Subject>
    {
        private readonly UniDbContext _dbContext;

        public FindSubjectByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Subject> Handle(
            FindSubjectByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var subject = await _dbContext
                        .Subjects
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.SubjectId, cancellationToken);
                    
                    transaction.Commit();
                    return subject;
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
