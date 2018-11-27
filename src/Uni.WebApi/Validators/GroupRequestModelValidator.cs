using FluentValidation;
using JetBrains.Annotations;
using Uni.Core;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
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
                .InclusiveBetween(1, 5);
        }
    }
}
