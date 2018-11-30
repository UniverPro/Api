using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Schedules.FindSchedules
{
    public class FindSchedulesQuery : IQuery<IEnumerable<Schedule>>
    {
    }
}
