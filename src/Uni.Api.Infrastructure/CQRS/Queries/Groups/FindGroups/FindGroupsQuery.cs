using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.Api.DataAccess.Models;
using Uni.Api.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Api.Infrastructure.CQRS.Queries.Groups.FindGroups
{
    public class FindGroupsQuery : IQuery<IEnumerable<Group>>
    {
        public FindGroupsQuery(
            int? facultyId,
            [CanBeNull] string name,
            int? courseNumber
            )
        {
            FacultyId = facultyId;
            Name = name;
            CourseNumber = courseNumber;
        }

        public int? FacultyId { get; }

        public string Name { get; }

        public int? CourseNumber { get; }

        [NotNull]
        public ISpecification<Group> ToSpecification()
        {
            var specification = Spec<Group>.New();

            if (FacultyId != null)
            {
                var facultyId = FacultyId.Value;
                specification = specification.And(Spec<Group>.New(x => x.FacultyId == facultyId));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                specification = specification.And(Spec<Group>.New(x => EF.Functions.Like(x.Name, $"%{Name}%")));
            }

            if (CourseNumber != null)
            {
                var courseNumber = CourseNumber.Value;
                specification = specification.And(Spec<Group>.New(x => x.CourseNumber == courseNumber));
            }

            return specification;
        }
    }
}
