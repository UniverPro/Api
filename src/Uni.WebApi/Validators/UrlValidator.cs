using System;
using FluentValidation.Validators;

namespace Uni.WebApi.Validators
{
    // TODO: Move this to Core project.
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
}
