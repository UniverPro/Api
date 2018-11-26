using FluentValidation;
using JetBrains.Annotations;
using Uni.DataAccess;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class FacultyRequestModelValidator : AbstractValidator<FacultyRequestModel>
    {
        public FacultyRequestModelValidator()
        {
            RuleFor(x => x.UniversityId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            RuleFor(x => x.ShortName)
                .MaximumLength(Consts.MaxShortNameLength)
                .When(x => !string.IsNullOrWhiteSpace(x.ShortName));
        }
    }
}
