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
            RuleFor(x => x.UniversityId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
