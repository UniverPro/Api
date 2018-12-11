using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Schedules.FindScheduleById
{
    [UsedImplicitly]
    public class FindScheduleByIdQueryHandler : IQueryHandler<FindScheduleByIdQuery, Schedule>
    {
        private readonly UniDbContext _dbContext;

        public FindScheduleByIdQueryHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Schedule> Handle(
            FindScheduleByIdQuery query,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction =
                await _dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead, cancellationToken))
            {
                try
                {
                    var schedule = await _dbContext
                        .Schedules
                        .AsNoTracking()
                        .SingleOrDefaultAsync(x => x.Id == query.ScheduleId, cancellationToken);

                    transaction.Commit();
                    return schedule;
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
