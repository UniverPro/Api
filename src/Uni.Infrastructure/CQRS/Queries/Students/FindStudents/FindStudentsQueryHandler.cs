using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.Core.Exceptions;
using Uni.DataAccess;
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

            var specification = query.ToSpecification();

            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                if (query.GroupId != null)
                {
                    var groupId = query.GroupId.Value;

                    var groupExists = await _dbContext
                        .Groups
                        .AsNoTracking()
                        .AnyAsync(x => x.Id == groupId, cancellationToken);

                    if (!groupExists)
                    {
                        throw new NotFoundException("group", groupId);
                    }
                }

                try
                {
                    var students = await _dbContext
                        .Students
                        .IncludeDefault()
                        .AsNoTracking()
                        .ExeSpec(specification)
                        .ToListAsync(cancellationToken);

                    transaction.Commit();
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
