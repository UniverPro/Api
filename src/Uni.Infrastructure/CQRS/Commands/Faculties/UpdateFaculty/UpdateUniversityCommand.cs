using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Faculties.UpdateFaculty
{
    public class UpdateFacultyCommand : ICommand
    {
        public UpdateFacultyCommand(
            int id,
            string name,
            string shortName,
            string description,
            int universityId
            )
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            Description = description;
            UniversityId = universityId;
        }

        public int Id { get; }

        public string Name { get; }

        public string ShortName { get; }

        public string Description { get; }

        public int UniversityId { get; }
    }
}
