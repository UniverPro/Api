using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Groups.CreateGroup
{
    public class CreateGroupCommand : ICommand<int>
    {
        public CreateGroupCommand(
            string name,
            int facultyId,
            int courseNumber
            )
        {
            Name = name;
            FacultyId = facultyId;
            CourseNumber = courseNumber;
        }

        public string Name { get; }

        public int FacultyId { get; }

        public int CourseNumber { get; }
    }
}
