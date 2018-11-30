using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Teachers.FindTeachers
{
    public class FindTeachersQuery : IQuery<IEnumerable<Teacher>>
    {
    }
}
