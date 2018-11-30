using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Subjects.CreateSubject
{
    public class CreateSubjectCommand : ICommand<int>
    {
        public CreateSubjectCommand(int groupId, string name)
        {
            GroupId = groupId;
            Name = name;
        }

        public int GroupId { get; }

        public string Name { get; }
    }
}
