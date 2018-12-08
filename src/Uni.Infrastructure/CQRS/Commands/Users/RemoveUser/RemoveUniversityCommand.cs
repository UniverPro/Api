using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Users.RemoveUser
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
