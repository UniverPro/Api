using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Subjects.FindSubjects
{
    public class FindSubjectsQuery : IQuery<IEnumerable<Subject>>
    {
        public FindSubjectsQuery(int? groupId, [CanBeNull] string name)
        {
            GroupId = groupId;
            Name = name;
        }

        public string Name { get; }

        public int? GroupId { get; }

        [NotNull]
        public ISpecification<Subject> ToSpecification()
        {
            var specification = Spec<Subject>.New();

            if (GroupId != null)
            {
                var groupId = GroupId.Value;
                specification = specification.And(Spec<Subject>.New(x => x.GroupId == groupId));
            }

            if (!string.IsNullOrEmpty(Name))
            {
                specification = specification.And(Spec<Subject>.New(x => EF.Functions.Like(x.Name, $"%{Name}%")));
            }

            return specification;
        }
    }
}
