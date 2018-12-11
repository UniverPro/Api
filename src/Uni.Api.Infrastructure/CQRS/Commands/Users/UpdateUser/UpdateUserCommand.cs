using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Users.UpdateUser
{
    public class UpdateUserCommand : ICommand
    {
        public UpdateUserCommand(
            int id,
            string login,
            string password,
            int personId
            )
        {
            Id = id;
            Login = login;
            Password = password;
            PersonId = personId;
        }

        public int Id { get; }

        public string Login { get; }

        public string Password { get; }

        public int PersonId { get; }
    }
}
