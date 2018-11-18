using System;
using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class StudentRequestModelValidator : AbstractValidator<StudentRequestModel>
    {
        public StudentRequestModelValidator([NotNull] PersonRequestModelValidator personRequestModelValidator)
        {
            if (personRequestModelValidator == null)
            {
                throw new ArgumentNullException(nameof(personRequestModelValidator));
            }

            Include(personRequestModelValidator);

            RuleFor(x => x.GroupId).NotEmpty();
        }
    }
}
