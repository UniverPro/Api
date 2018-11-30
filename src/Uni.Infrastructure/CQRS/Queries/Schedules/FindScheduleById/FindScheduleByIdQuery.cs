using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Schedules.FindScheduleById
{
    public class FindScheduleByIdQuery : IQuery<Schedule>
    {
        public FindScheduleByIdQuery(int scheduleId)
        {
            ScheduleId = scheduleId;
        }

        public int ScheduleId { get; }
    }
}
