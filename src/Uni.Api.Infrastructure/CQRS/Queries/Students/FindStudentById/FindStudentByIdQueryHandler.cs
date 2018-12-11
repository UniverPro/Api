using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.Api.DataAccess;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Students.FindStudentById
{
    [UsedImplicitly]
    public class FindStudentByIdQueryHandler : IQueryHandler<FindStudentByIdQuery, Student>
    {
        private readonly UniDbContext _dbContext;

        public FindStudentByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Student> Handle(
            FindStudentByIdQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var student = await _dbContext
                        .Students
                        .IncludeDefault()
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.StudentId, cancellationToken);

                    transaction.Commit();
                    return student;
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
