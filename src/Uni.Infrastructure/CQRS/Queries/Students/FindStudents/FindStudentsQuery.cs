using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using LinqBuilder.OrderBy;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Students.FindStudents
{
    public class FindStudentsQuery : IQuery<IEnumerable<Student>>
    {
        public FindStudentsQuery(
            int? groupId,
            [CanBeNull] string firstName,
            [CanBeNull] string lastName,
            [CanBeNull] string middleName,
            [CanBeNull] string email,
            [CanBeNull] string avatarPath
            )
        {
            GroupId = groupId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            AvatarPath = avatarPath;
        }

        public int? GroupId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string Email { get; }

        public string AvatarPath { get; }

        [NotNull]
        public ISpecification<Student> ToSpecification()
        {
            var specification = Spec<Student>.New();

            if (GroupId != null)
            {
                var groupId = GroupId.Value;
                specification = specification.And(Spec<Student>.New(x => x.GroupId == groupId));
            }

            if (!string.IsNullOrEmpty(FirstName))
            {
                specification = specification.And(
                    Spec<Student>.New(x => EF.Functions.Like(x.FirstName, $"%{FirstName}%"))
                );
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                specification =
                    specification.And(Spec<Student>.New(x => EF.Functions.Like(x.LastName, $"%{LastName}%")));
            }

            if (!string.IsNullOrEmpty(MiddleName))
            {
                specification = specification.And(
                    Spec<Student>.New(x => EF.Functions.Like(x.MiddleName, $"%{MiddleName}%"))
                );
            }
            
            if (!string.IsNullOrEmpty(Email))
            {
                specification = specification.And(
                    Spec<Student>.New(x => EF.Functions.Like(x.Email, $"%{Email}%"))
                );
            }

            if (!string.IsNullOrEmpty(AvatarPath))
            {
                specification = specification.And(
                    Spec<Student>.New(x => EF.Functions.Like(x.AvatarPath, $"%{AvatarPath}%"))
                );
            }

            var lastNameAscending = OrderSpec<Student, string>.New(p => p.LastName);
            var firstNameAscending = OrderSpec<Student, string>.New(p => p.FirstName);
            var orderSpecification = lastNameAscending.ThenBy(firstNameAscending);

            specification = specification.UseOrdering(orderSpecification);

            return specification;
        }
    }
}
