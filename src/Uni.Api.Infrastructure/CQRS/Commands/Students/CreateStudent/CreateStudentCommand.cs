using Microsoft.AspNetCore.Http;
using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Students.CreateStudent
{
    public class CreateStudentCommand : ICommand<int>
    {
        public CreateStudentCommand(
            string firstName,
            string lastName,
            string middleName,
            string email,
            IFormFile avatar,
            int groupId
            )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            Avatar = avatar;
            GroupId = groupId;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string Email { get; }

        public IFormFile Avatar { get; }

        public int GroupId { get; }
    }
}
