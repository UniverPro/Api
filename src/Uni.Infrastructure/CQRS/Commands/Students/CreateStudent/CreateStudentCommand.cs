using Microsoft.AspNetCore.Http;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Students.CreateStudent
{
    public class CreateStudentCommand : ICommand<int>
    {
        public CreateStudentCommand(
            string firstName,
            string lastName,
            string middleName,
            IFormFile avatar,
            int groupId
            )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Avatar = avatar;
            GroupId = groupId;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public IFormFile Avatar { get; }

        public int GroupId { get; }
    }
}
