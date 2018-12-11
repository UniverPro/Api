using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Users.FindUserById
{
    public class FindUserByIdQuery : IQuery<User>
    {
        public FindUserByIdQuery(int userId)
        {
            UserId = userId;
        }

        public int UserId { get; }
    }
}
