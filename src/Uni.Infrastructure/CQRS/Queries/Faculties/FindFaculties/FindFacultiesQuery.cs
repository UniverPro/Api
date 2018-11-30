using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Faculties.FindFaculties
{
    public class FindFacultiesQuery : IQuery<IEnumerable<Faculty>>
    {
    }
}
