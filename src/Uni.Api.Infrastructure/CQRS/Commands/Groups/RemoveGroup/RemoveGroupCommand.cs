using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Groups.RemoveGroup
{
    public class RemoveGroupCommand : ICommand
    {
        public RemoveGroupCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
