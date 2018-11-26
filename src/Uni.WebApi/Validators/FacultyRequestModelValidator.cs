using FluentValidation;
using JetBrains.Annotations;
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
                .MaximumLength(255);

            RuleFor(x => x.ShortName)
                .MaximumLength(16)
                .When(x => !string.IsNullOrWhiteSpace(x.ShortName));
        }
    }
}
