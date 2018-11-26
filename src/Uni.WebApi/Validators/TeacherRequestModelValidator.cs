using System;
using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
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
