using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Teachers.RemoveTeacher
{
    public class RemoveTeacherCommand : ICommand
    {
        public RemoveTeacherCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
