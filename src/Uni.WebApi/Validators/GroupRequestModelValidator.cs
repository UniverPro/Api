using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class GroupRequestModelValidator : AbstractValidator<GroupRequestModel>
    {
        public GroupRequestModelValidator()
        {
            RuleFor(x => x.FacultyId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CourseNumber).InclusiveBetween(1, 5);
        }
    }
}
