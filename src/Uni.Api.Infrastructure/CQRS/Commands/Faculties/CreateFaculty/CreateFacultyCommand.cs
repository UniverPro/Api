using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Faculties.CreateFaculty
{
    public class CreateFacultyCommand : ICommand<int>
    {
        public CreateFacultyCommand(
            string name,
            string shortName,
            string description,
            int universityId
            )
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            UniversityId = universityId;
        }

        public string Name { get; }

        public string ShortName { get; }

        public string Description { get; }

        public int UniversityId { get; }
    }
}
