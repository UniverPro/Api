using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Groups.FindGroupById
{
    public class FindGroupByIdQuery : IQuery<Group>
    {
        public FindGroupByIdQuery(int groupId)
        {
            GroupId = groupId;
        }

        public int GroupId { get; }
    }
}
