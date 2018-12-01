using FluentValidation;
using JetBrains.Annotations;
using Uni.Core;
using Uni.Core.Extensions;
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

            RuleFor(x => x.AvatarPath)
                .IsValidUrl().When(x => !string.IsNullOrEmpty(x.AvatarPath));
        }
    }
}
