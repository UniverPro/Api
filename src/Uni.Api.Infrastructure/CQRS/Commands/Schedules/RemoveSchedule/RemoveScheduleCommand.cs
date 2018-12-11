using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Schedules.RemoveSchedule
{
    public class RemoveScheduleCommand : ICommand
    {
        public RemoveScheduleCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
