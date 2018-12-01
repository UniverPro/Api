using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjects
{
    [UsedImplicitly]
    public class FindSubjectsQueryHandler : IQueryHandler<FindSubjectsQuery, IEnumerable<Subject>>
    {
        private readonly UniDbContext _dbContext;

        public FindSubjectsQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Subject>> Handle(
            FindSubjectsQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var subjects = await _dbContext
                        .Subjects
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);
                    
                    transaction.Commit();
                    return subjects;
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
