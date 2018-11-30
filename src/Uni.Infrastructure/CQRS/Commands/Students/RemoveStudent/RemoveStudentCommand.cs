using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Students.RemoveStudent
{
    public class RemoveStudentCommand : ICommand
    {
        public RemoveStudentCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
