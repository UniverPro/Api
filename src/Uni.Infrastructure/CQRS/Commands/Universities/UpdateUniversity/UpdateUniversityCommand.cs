using Uni.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Infrastructure.CQRS.Commands.Universities.UpdateUniversity
{
    public class UpdateUniversityCommand : ICommand
    {
        public UpdateUniversityCommand(int id, string name, string shortName, string description)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            Description = description;
        }

        public int Id { get; }

        public string Name { get; }

        public string ShortName { get; }

        public string Description { get; }
    }
}
