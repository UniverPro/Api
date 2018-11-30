using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Universities.RemoveUniversity
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
