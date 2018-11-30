using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Groups.UpdateGroup
{
    public class UpdateGroupCommand : ICommand
    {
        public UpdateGroupCommand(
            int id,
            string name,
            int facultyId,
            int courseNumber
            )
        {
            Id = id;
            Name = name;
            FacultyId = facultyId;
            CourseNumber = courseNumber;
        }

        public int Id { get; }
        
        public string Name { get; }

        public int FacultyId { get; }

        public int CourseNumber { get; }
    }
}
