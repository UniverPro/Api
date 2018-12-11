using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Universities.RemoveUniversity
{
    public class RemoveUniversityCommand : ICommand
    {
        public RemoveUniversityCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
