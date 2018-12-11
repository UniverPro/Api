using System;
using FluentValidation;
using JetBrains.Annotations;
using Uni.Api.Core.Validators.PropertyValidators;

namespace Uni.Api.Core.Extensions
{
    public static class CustomValidatorsExtensions
    {
        [NotNull]
        public static IRuleBuilderOptions<T, string> IsValidUrl<T>([NotNull] this IRuleBuilder<T, string> ruleBuilder)
        {
            if (ruleBuilder == null)
            {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            var urlValidator = new UrlValidator();
            return ruleBuilder.SetValidator(urlValidator);
        }
    }
}
