using System;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
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

        [NotNull]
        public ISpecification<User> ToSpecification()
        {
            var specification = Spec<User>.New();
            
            specification = specification.And(Spec<User>.New(x => EF.Functions.Like(x.Login, Login)));
            specification = specification.And(Spec<User>.New(x => EF.Functions.Like(x.Password, Password)));

            return specification;
        }
    }
}
