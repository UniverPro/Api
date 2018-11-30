using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.UpdateTeacher
{
    public class UpdateTeacherCommand : ICommand
    {
        public UpdateTeacherCommand(
            int id,
            string firstName,
            string lastName,
            string middleName,
            string avatarPath,
            int facultyId
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            AvatarPath = avatarPath;
            FacultyId = facultyId;
        }

        public int Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string AvatarPath { get; }

        public int FacultyId { get; }
    }
}
