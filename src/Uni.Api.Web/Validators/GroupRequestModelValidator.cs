using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Core;
using Uni.Api.Shared.Requests;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class GroupRequestModelValidator : AbstractValidator<GroupRequestModel>
    {
        public GroupRequestModelValidator()
        {
            RuleFor(x => x.FacultyId)
                .NotEmpty();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            RuleFor(x => x.CourseNumber)
                .NotEmpty()
                .InclusiveBetween(1, 5);
        }
    }
}
