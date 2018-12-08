using System.Collections.Generic;
using JetBrains.Annotations;
using LinqBuilder;
using LinqBuilder.Core;
using LinqBuilder.OrderBy;
using Microsoft.EntityFrameworkCore;
using Uni.DataAccess.Models;
using Uni.Infrastructure.Interfaces.CQRS.Queries;

namespace Uni.Infrastructure.CQRS.Queries.Teachers.FindTeachers
{
    public class FindTeachersQuery : IQuery<IEnumerable<Teacher>>
    {
        public FindTeachersQuery(
            int? facultyId,
            [CanBeNull] string firstName,
            [CanBeNull] string lastName,
            [CanBeNull] string middleName,
            [CanBeNull] string email,
            [CanBeNull] string avatarPath
            )
        {
            FacultyId = facultyId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            AvatarPath = avatarPath;
        }

        public int? FacultyId { get; }

        public string FirstName { get; }

        public string LastName { get; }

        public string MiddleName { get; }

        public string Email { get; }

        public string AvatarPath { get; }

        [NotNull]
        public ISpecification<Teacher> ToSpecification()
        {
            var specification = Spec<Teacher>.New();

            if (FacultyId != null)
            {
                var facultyId = FacultyId.Value;
                specification = specification.And(Spec<Teacher>.New(x => x.FacultyId == facultyId));
            }

            if (!string.IsNullOrEmpty(FirstName))
            {
                specification = specification.And(
                    Spec<Teacher>.New(x => EF.Functions.Like(x.FirstName, $"%{FirstName}%"))
                );
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                specification =
                    specification.And(Spec<Teacher>.New(x => EF.Functions.Like(x.LastName, $"%{LastName}%")));
            }

            if (!string.IsNullOrEmpty(MiddleName))
            {
                specification = specification.And(
                    Spec<Teacher>.New(x => EF.Functions.Like(x.MiddleName, $"%{MiddleName}%"))
                );
            }
            
            if (!string.IsNullOrEmpty(Email))
            {
                specification = specification.And(
                    Spec<Teacher>.New(x => EF.Functions.Like(x.Email, $"%{Email}%"))
                );
            }

            if (!string.IsNullOrEmpty(AvatarPath))
            {
                specification = specification.And(
                    Spec<Teacher>.New(x => EF.Functions.Like(x.AvatarPath, $"%{AvatarPath}%"))
                );
            }

            var lastNameAscending = OrderSpec<Teacher, string>.New(p => p.LastName);
            var firstNameAscending = OrderSpec<Teacher, string>.New(p => p.FirstName);
            var orderSpecification = lastNameAscending.ThenBy(firstNameAscending);

            specification = specification.UseOrdering(orderSpecification);

            return specification;
        }
    }
}
