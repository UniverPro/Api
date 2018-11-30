using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjects
{
    public class FindSubjectsQuery : IQuery<IEnumerable<Subject>>
    {
    }
}
