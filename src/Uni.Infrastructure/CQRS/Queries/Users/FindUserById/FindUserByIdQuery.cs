using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Users.FindUserById
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
