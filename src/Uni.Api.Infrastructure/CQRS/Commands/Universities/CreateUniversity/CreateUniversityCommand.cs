using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Universities.CreateUniversity
{
    public class CreateUniversityCommand : ICommand<int>
    {
        public CreateUniversityCommand(
            string name,
            string shortName,
            string description
            )
        {
            Name = name;
            ShortName = shortName;
            Description = description;
        }

        public string Name { get; }

        public string ShortName { get; }

        public string Description { get; }
    }
}
