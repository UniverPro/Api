using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class PersonRequestModelValidator : AbstractValidator<PersonRequestModel>
    {
        public PersonRequestModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(255);

            When(x => !string.IsNullOrEmpty(x.MiddleName), () =>
                RuleFor(x => x.MiddleName)
                    .NotEmpty()
                    .MaximumLength(255));

            RuleFor(x => x.AvatarPath)
                .IsValidUrl().When(x => string.IsNullOrEmpty(x.AvatarPath));
        }
    }
}
