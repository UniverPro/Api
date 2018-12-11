using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Subjects.RemoveSubject
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
