using Microsoft.AspNetCore.Http;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Teachers.CreateTeacher
{
    public class CreateTeacherCommand : ICommand<int>
    {
        public CreateTeacherCommand(
            string firstName,
            string lastName,
            string middleName,
            string email,
            IFormFile avatar,
            int facultyId
            )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            Avatar = avatar;
            FacultyId = facultyId;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string Email { get; }

        public IFormFile Avatar { get; }

        public int FacultyId { get; }
    }
}
