using System.Collections.Generic;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Groups.FindGroups
{
    public class FindGroupsQuery : IQuery<IEnumerable<Group>>
    {
    }
}
