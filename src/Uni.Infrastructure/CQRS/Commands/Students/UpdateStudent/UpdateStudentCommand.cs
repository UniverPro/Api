using Microsoft.AspNetCore.Http;
using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Students.UpdateStudent
{
    public class UpdateStudentCommand : ICommand
    {
        public UpdateStudentCommand(
            int id,
            string firstName,
            string lastName,
            string middleName,
            IFormFile avatar,
            int groupId
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Avatar = avatar;
            GroupId = groupId;
        }

        public int Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public IFormFile Avatar { get; }

        public int GroupId { get; }
    }
}
