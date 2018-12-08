using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Web.Models.Requests;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class ScheduleRequestModelValidator : AbstractValidator<ScheduleRequestModel>
    {
        public ScheduleRequestModelValidator()
        {
            RuleFor(x => x.SubjectId)
                .NotEmpty();

            RuleFor(x => x.TeacherId)
                .NotEmpty();
            
            RuleFor(x => x.StartTime)
                .NotEmpty();

            RuleFor(x => x.Duration)
                .NotEmpty();
        }
    }
}
