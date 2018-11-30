using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.FindUniversities
{
    public class FindUniversitiesQuery : IQuery<IEnumerable<University>>
    {
    }
}
