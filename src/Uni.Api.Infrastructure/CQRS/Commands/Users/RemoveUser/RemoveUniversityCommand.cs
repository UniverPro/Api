using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Users.RemoveUser
{
    public class RemoveUserCommand : ICommand
    {
        public RemoveUserCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
