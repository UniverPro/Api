using System;
using JetBrains.Annotations;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Users.FindUserByLoginAndPassword
{
    public class FindUserByLoginAndPasswordQuery : IQuery<User>
    {
        public FindUserByLoginAndPasswordQuery([NotNull] string login, [NotNull] string password)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(login));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            }

            Login = login;
            Password = password;
        }
        
        [NotNull]
        public string Login { get; }

        [NotNull]
        public string Password { get; }
    }
}
