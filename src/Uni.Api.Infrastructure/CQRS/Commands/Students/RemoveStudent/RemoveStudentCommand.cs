using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Students.RemoveStudent
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
