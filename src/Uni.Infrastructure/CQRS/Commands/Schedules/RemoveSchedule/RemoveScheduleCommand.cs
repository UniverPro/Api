using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Schedules.RemoveSchedule
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
