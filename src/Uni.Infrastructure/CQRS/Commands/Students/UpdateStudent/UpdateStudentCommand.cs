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
            string avatarPath,
            int groupId
            )
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            AvatarPath = avatarPath;
            GroupId = groupId;
        }

        public int Id { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string AvatarPath { get; }

        public int GroupId { get; }
    }
}
