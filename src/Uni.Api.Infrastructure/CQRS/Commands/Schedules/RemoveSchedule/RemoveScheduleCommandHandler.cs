using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.Api.Core.Exceptions;
using Uni.Api.DataAccess.Contexts;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Schedules.RemoveSchedule
{
    [UsedImplicitly]
    public class RemoveScheduleCommandHandler : ICommandHandler<RemoveScheduleCommand>
    {
        private readonly UniDbContext _dbContext;

        public RemoveScheduleCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            RemoveScheduleCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var schedule =
                        await _dbContext.Schedules.SingleOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

                    if (schedule == null)
                    {
                        throw new NotFoundException(nameof(schedule), command.Id);
                    }

                    _dbContext.Remove(schedule);

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    transaction.Commit();

                    return Unit.Value;
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
