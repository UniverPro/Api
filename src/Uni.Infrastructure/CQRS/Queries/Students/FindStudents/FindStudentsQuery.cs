using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Students.FindStudents
{
    public class FindStudentsQuery : IQuery<IEnumerable<Student>>
    {
    }
}
