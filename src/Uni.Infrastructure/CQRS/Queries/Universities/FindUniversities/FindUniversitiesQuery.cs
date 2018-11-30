using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.FindUniversities
{
    public class FindUniversitiesQuery : IQuery<IEnumerable<University>>
    {
        public FindUniversitiesQuery(
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

        public Expression<Func<University, bool>> ToSpecification()
        {
            var predicate = PredicateBuilder.New<University>(true);

            if (!string.IsNullOrEmpty(Name))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.Name, $"%{Name}%"));
            }

            if (!string.IsNullOrEmpty(ShortName))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.ShortName, $"%{ShortName}%"));
            }

            if (!string.IsNullOrEmpty(Description))
            {
                predicate = predicate.And(x => EF.Functions.Like(x.Description, $"%{Description}%"));
            }

            return predicate;
        }
    }
}
