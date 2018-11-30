using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Subjects.RemoveSubject
{
    public class RemoveSubjectCommand : ICommand
    {
        public RemoveSubjectCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
