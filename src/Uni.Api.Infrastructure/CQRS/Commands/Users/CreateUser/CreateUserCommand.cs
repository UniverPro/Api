using Uni.Api.Infrastructure.Interfaces.CQRS.Commands;

namespace Uni.Api.Infrastructure.CQRS.Commands.Users.CreateUser
{
    public class CreateUserCommand : ICommand<int>
    {
        public CreateUserCommand(
            string login,
            string password,
            int personId
            )
        {
            Login = login;
            Password = password;
            PersonId = personId;
        }

        public string Login { get; }

        public string Password { get; }

        public int PersonId { get; }
    }
}
