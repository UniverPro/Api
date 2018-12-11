using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Core;
using Uni.Api.Shared.Requests;

namespace Uni.Api.Web.Validators
{
    [UsedImplicitly]
    public class UserRequestModelValidator : AbstractValidator<UserRequestModel>
    {
        public UserRequestModelValidator()
        {
            RuleFor(x => x.PersonId)
                .NotEmpty();

            RuleFor(x => x.Login)
                .NotEmpty()
                .MaximumLength(Consts.MaxLoginLength);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(Consts.MaxPasswordLength);
        }
    }
}
