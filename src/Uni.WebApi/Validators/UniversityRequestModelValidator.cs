using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class UniversityRequestModelValidator : AbstractValidator<UniversityRequestModel>
    {
        public UniversityRequestModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
