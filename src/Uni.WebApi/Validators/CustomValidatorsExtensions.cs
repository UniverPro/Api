using FluentValidation;

namespace Uni.WebApi.Validators
{
    // TODO: Move this to Core project.
    public static class CustomValidatorsExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UrlValidator());
        }
    }
}
