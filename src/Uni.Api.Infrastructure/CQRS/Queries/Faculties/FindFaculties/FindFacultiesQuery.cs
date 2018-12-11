using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Faculties.FindFaculties
{
    public class FindFacultiesQuery : IQuery<IEnumerable<Faculty>>
    {
        public FindFacultiesQuery(
            int? universityId,
            [CanBeNull] string name,
            [CanBeNull] string shortName,
            [CanBeNull] string description
            )
        {
            Name = name;
            ShortName = shortName;
            Description = description;
            UniversityId = universityId;
        }

        public int? UniversityId { get; }

        public string Name { get; }

        public string ShortName { get; }

        public string Description { get; }

        [NotNull]
        public ISpecification<Faculty> ToSpecification()
        {
            var specification = Spec<Faculty>.New();

            if (UniversityId != null)
            {
                var universityId = UniversityId.Value;
                specification = specification.And(Spec<Faculty>.New(x => x.UniversityId == universityId));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                specification = specification.And(Spec<Faculty>.New(x => EF.Functions.Like(x.Name, $"%{Name}%")));
            }

            if (!string.IsNullOrEmpty(ShortName))
            {
                specification = specification.And(
                    Spec<Faculty>.New(x => EF.Functions.Like(x.ShortName, $"%{ShortName}%"))
                );
            }

            if (!string.IsNullOrEmpty(Description))
            {
                specification = specification.And(
                    Spec<Faculty>.New(x => EF.Functions.Like(x.Description, $"%{Description}%"))
                );
            }

            return specification;
        }
    }
}
