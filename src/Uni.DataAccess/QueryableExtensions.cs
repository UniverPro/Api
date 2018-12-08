using System;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;

namespace Uni.DataAccess
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IncludeDefault<T>([NotNull] this IQueryable<T> items) where T : Person
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            return items.Include(x => x.User);
        }
    }
}