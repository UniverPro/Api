using FluentValidation;
using JetBrains.Annotations;
using Uni.Core;
using Uni.Api.Web.Models.Requests;

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
