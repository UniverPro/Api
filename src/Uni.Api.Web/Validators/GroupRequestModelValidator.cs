using FluentValidation;
using JetBrains.Annotations;
using Uni.Core;
using Uni.Api.Web.Models.Requests;

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
