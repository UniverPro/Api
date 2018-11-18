using System;
using FluentValidation;
using FluentValidation.Validators;
using JetBrains.Annotations;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    public static class CustomValidatorsExtensions
    {
        public static IRuleBuilderOptions<T, string> IsValidUrl<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new UrlValidator());
        }
    }

    public class UrlValidator : PropertyValidator
    {
        public UrlValidator()
            : base("{PropertyName} should be a valid URL")
        {
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (!(context.PropertyValue is string value))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (Uri.TryCreate(value, UriKind.Absolute, out var uri))
            {
                return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
            }

            return false;
        }
    }

    [UsedImplicitly]
    public class PersonRequestModelValidator : AbstractValidator<PersonRequestModel>
    {
        public PersonRequestModelValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty().Unless(x => string.IsNullOrEmpty(x.MiddleName));
            RuleFor(x => x.AvatarPath).IsValidUrl().Unless(x => string.IsNullOrEmpty(x.AvatarPath));
        }
    }
}
