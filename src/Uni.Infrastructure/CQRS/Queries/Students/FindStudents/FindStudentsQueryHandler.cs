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

namespace Uni.Infrastructure.CQRS.Queries.Students.FindStudents
{
    [UsedImplicitly]
    public class FindStudentsQueryHandler : IQueryHandler<FindStudentsQuery, IEnumerable<Student>>
    {
        private readonly UniDbContext _dbContext;

        public FindStudentsQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Student>> Handle(
            FindStudentsQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var students = await _dbContext
                        .Students
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    return students;
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
