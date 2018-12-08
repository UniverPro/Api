using System;
using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Shared.Requests;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class TeacherRequestModelValidator : AbstractValidator<TeacherRequestModel>
    {
        public TeacherRequestModelValidator([NotNull] PersonRequestModelValidator personRequestModelValidator)
        {
            if (personRequestModelValidator == null)
            {
                throw new ArgumentNullException(nameof(personRequestModelValidator));
            }

            Include(personRequestModelValidator);

            RuleFor(x => x.FacultyId)
                .NotEmpty();
        }
    }
}
