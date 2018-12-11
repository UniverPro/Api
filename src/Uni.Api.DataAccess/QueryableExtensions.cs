using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.Api.DataAccess.Models;

namespace Uni.Api.DataAccess
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeDefault<T>([NotNull] this IQueryable<T> persons) where T : Person
        {
            if (persons == null)
            {
                throw new ArgumentNullException(nameof(persons));
            }

            return persons.Include(x => x.User);
        }

        public static IQueryable<User> IncludeDefault([NotNull] this IQueryable<User> users)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }

            return users
                .Include(x => x.Person)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission);
        }
    }
}
