using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.CreateTeacher
{
    public class CreateTeacherCommand : ICommand<int>
    {
        public CreateTeacherCommand(
            string firstName,
            string lastName,
            string middleName,
            string avatarPath,
            int facultyId
            )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            AvatarPath = avatarPath;
            FacultyId = facultyId;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string AvatarPath { get; }

        public int FacultyId { get; }
    }
}
