using FluentValidation;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
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

            RuleFor(x => x.AudienceNumber)
                .NotEmpty();
        }
    }
}
