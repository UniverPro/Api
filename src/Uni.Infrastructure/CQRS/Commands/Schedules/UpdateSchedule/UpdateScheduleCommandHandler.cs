using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Contexts;
using Uni.Infrastructure.Exceptions;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Schedules.UpdateSchedule
{
    [UsedImplicitly]
    public class UpdateScheduleCommandHandler : ICommandHandler<UpdateScheduleCommand>
    {
        private readonly UniDbContext _dbContext;

        public UpdateScheduleCommandHandler([NotNull] UniDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Unit> Handle(
            UpdateScheduleCommand command,
            CancellationToken cancellationToken
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var schedule = await _dbContext.Schedules.SingleOrDefaultAsync(
                        x => x.Id == command.Id,
                        cancellationToken
                    );

                    if (schedule == null)
                    {
                        throw new NotFoundException();
                    }

                    schedule.SubjectId = command.SubjectId;
                    schedule.TeacherId = command.TeacherId;
                    schedule.StartTime = command.StartTime;
                    schedule.Duration = command.Duration;
                    schedule.LessonType = command.LessonType;
                    schedule.AudienceNumber = command.AudienceNumber;

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
