﻿using FluentValidation;
using JetBrains.Annotations;
using Uni.Core;
using Uni.WebApi.Models.Requests;

namespace Uni.WebApi.Validators
{
    [UsedImplicitly]
    public class SubjectRequestModelValidator : AbstractValidator<SubjectRequestModel>
    {
        public SubjectRequestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(Consts.MaxNameLength);

            RuleFor(x => x.GroupId)
                .NotEmpty();
        }
    }
}
