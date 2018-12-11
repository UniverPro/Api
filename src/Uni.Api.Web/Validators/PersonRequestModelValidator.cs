using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Core;
using Uni.Api.Shared.Requests;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class PersonRequestModelValidator : AbstractValidator<PersonRequestModel>
    {
        public PersonRequestModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            When(
                x => !string.IsNullOrEmpty(x.MiddleName),
                () =>
                    RuleFor(x => x.MiddleName)
                        .NotEmpty()
                        .MaximumLength(Consts.MaxNameLength)
            );

            When(
                x => !string.IsNullOrEmpty(x.Email),
                () =>
                    RuleFor(x => x.Email)
                        .NotEmpty()
                        .MaximumLength(Consts.MaxEmailLength)
                        .EmailAddress()
            );
        }
    }
}
