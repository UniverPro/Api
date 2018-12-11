using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Core;
using Uni.Api.Shared.Requests;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class SubjectRequestModelValidator : AbstractValidator<SubjectRequestModel>
    {
        public SubjectRequestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            RuleFor(x => x.GroupId)
                .NotEmpty();
        }
    }
}
