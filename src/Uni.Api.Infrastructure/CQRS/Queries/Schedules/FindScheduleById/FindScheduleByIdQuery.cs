using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Schedules.FindScheduleById
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
