using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class SubjectRequestModelValidator : AbstractValidator<SubjectRequestModel>
    {
        public SubjectRequestModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.GroupId).NotEmpty();
            RuleFor(x => x.TeacherId).NotEmpty();
        }
    }
}
