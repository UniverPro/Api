using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.DataAccess.Models;
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
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var schedules = await _dbContext
                        .Schedules
                        .AsNoTracking()
                        .OrderBy(x => x.StartTime)
                        .ToListAsync(cancellationToken);

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
