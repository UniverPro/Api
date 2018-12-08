using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Shared.Requests;
using Uni.Core;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class UniversityRequestModelValidator : AbstractValidator<UniversityRequestModel>
    {
        public UniversityRequestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            RuleFor(x => x.ShortName)
                .MaximumLength(Consts.MaxShortNameLength)
                .When(x => !string.IsNullOrEmpty(x.ShortName));
        }
    }
}
