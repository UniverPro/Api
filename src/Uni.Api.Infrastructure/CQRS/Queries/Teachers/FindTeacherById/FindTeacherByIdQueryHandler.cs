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

namespace Uni.Api.Infrastructure.CQRS.Queries.Teachers.FindTeacherById
{
    [UsedImplicitly]
    public class FindTeacherByIdQueryHandler : IQueryHandler<FindTeacherByIdQuery, Teacher>
    {
        private readonly UniDbContext _dbContext;

        public FindTeacherByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Teacher> Handle(
            FindTeacherByIdQuery query,
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
                        .IncludeDefault()
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.TeacherId, cancellationToken);

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
