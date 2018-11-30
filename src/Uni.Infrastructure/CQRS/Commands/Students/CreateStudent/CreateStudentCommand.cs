using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Students.CreateStudent
{
    public class CreateStudentCommand : ICommand<int>
    {
        public CreateStudentCommand(
            string firstName,
            string lastName,
            string middleName,
            string avatarPath,
            int groupId
            )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            AvatarPath = avatarPath;
            GroupId = groupId;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string AvatarPath { get; }

        public int GroupId { get; }
    }
}
