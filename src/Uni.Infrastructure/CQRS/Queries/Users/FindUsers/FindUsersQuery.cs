using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Users.FindUsers
{
    public class FindUsersQuery : IQuery<IEnumerable<User>>
    {
        public FindUsersQuery([CanBeNull] string login)
        {
            Login = login;
        }

        public string Login { get; }

        [NotNull]
        public ISpecification<User> ToSpecification()
        {
            var specification = Spec<User>.New();

            if (!string.IsNullOrEmpty(Login))
            {
                specification = specification.And(Spec<User>.New(x => EF.Functions.Like(x.Login, $"%{Login}%")));
            }

            return specification;
        }
    }
}
