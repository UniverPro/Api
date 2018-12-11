using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Faculties.RemoveFaculty
{
    public class RemoveFacultyCommand : ICommand
    {
        public RemoveFacultyCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }
    }
}
