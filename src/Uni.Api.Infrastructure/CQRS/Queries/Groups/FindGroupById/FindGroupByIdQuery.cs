using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Groups.FindGroupById
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
