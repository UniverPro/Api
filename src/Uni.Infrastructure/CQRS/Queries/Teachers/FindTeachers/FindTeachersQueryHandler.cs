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

namespace Uni.Infrastructure.CQRS.Queries.Teachers.FindTeachers
{
    [UsedImplicitly]
    public class FindTeachersQueryHandler : IQueryHandler<FindTeachersQuery, IEnumerable<Teacher>>
    {
        private readonly UniDbContext _dbContext;

        public FindTeachersQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Teacher>> Handle(
            FindTeachersQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var teachers = await _dbContext
                        .Teachers
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    return teachers;
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
