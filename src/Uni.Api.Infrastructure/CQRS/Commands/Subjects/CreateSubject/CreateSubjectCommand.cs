using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Subjects.CreateSubject
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
