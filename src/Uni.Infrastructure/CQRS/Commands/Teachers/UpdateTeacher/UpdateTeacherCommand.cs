using Microsoft.AspNetCore.Http;
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
            string email,
            IFormFile avatar,
            int facultyId
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            Avatar = avatar;
            FacultyId = facultyId;
        }

        public int Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string Email { get; }

        public IFormFile Avatar { get; }

        public int FacultyId { get; }
    }
}
