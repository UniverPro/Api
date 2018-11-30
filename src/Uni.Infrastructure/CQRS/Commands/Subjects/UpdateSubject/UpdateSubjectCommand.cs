using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Subjects.UpdateSubject
{
    public class UpdateSubjectCommand : ICommand
    {
        public UpdateSubjectCommand(
            int id,
            int groupId,
            string name
            )
        {
            Id = id;
            GroupId = groupId;
            Name = name;
        }

        public int Id { get; }

        public int GroupId { get; }

        public string Name { get; }
    }
}
