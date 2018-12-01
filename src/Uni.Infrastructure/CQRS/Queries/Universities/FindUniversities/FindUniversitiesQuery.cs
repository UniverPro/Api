using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Universities.FindUniversities
{
    public class FindUniversitiesQuery : IQuery<IEnumerable<University>>
    {
        public FindUniversitiesQuery(
            [CanBeNull] string name,
            [CanBeNull] string shortName,
            [CanBeNull] string description
            )
        {
            Name = name;
            ShortName = shortName;
            Description = description;
        }

        public string Name { get; }

        public string ShortName { get; }

        public string Description { get; }

        [NotNull]
        public ISpecification<University> ToSpecification()
        {
            var specification = Spec<University>.New();

            if (!string.IsNullOrEmpty(Name))
            {
                specification = specification.And(Spec<University>.New(x => EF.Functions.Like(x.Name, $"%{Name}%")));
            }

            if (!string.IsNullOrEmpty(ShortName))
            {
                specification = specification.And(
                    Spec<University>.New(x => EF.Functions.Like(x.ShortName, $"%{ShortName}%"))
                );
            }

            if (!string.IsNullOrEmpty(Description))
            {
                specification = specification.And(
                    Spec<University>.New(x => EF.Functions.Like(x.Description, $"%{Description}%"))
                );
            }

            return specification;
        }
    }
}
