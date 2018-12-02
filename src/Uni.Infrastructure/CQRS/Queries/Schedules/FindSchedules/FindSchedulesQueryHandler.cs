using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Exceptions;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Schedules.FindSchedules
{
    [UsedImplicitly]
    public class FindSchedulesQueryHandler : IQueryHandler<FindSchedulesQuery, IEnumerable<Schedule>>
    {
        private readonly UniDbContext _dbContext;

        public FindSchedulesQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Schedule>> Handle(
            FindSchedulesQuery query,
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
                    if (query.TeacherId != null)
                    {
                        var teacherId = query.TeacherId.Value;

                        var teacherExists = await _dbContext
                            .Teachers
                            .AsNoTracking()
                            .AnyAsync(x => x.Id == teacherId, cancellationToken);

                        if (!teacherExists)
                        {
                            throw new NotFoundException("teacher", teacherId);
                        }
                    }

                    if (query.SubjectId != null)
                    {
                        var subjectId = query.SubjectId.Value;

                        var subjectExists = await _dbContext
                            .Subjects
                            .AsNoTracking()
                            .AnyAsync(x => x.Id == subjectId, cancellationToken);

                        if (!subjectExists)
                        {
                            throw new NotFoundException("teacher", subjectId);
                        }
                    }

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

                    var schedules = await _dbContext
                        .Schedules
                        .AsNoTracking()
                        .Include(x => x.Subject)
                        .Include(x => x.Teacher)
                        .ExeSpec(specification)
                        .ToListAsync(cancellationToken);

                    transaction.Commit();
                    return schedules;
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
