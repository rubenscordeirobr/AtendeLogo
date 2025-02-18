﻿using System.Linq.Expressions;
using AtendeLogo.Common.Extensions;
using AtendeLogo.UseCases.Identities.Tenants.Commands;
using FluentValidation.Results;

namespace AtendeLogo.Application.UnitTests.Common;

public static partial class ValidationResultExtensions
{
    public static void ShouldHaveValidationErrorFor<T>(
        this ValidationResult result,
        Expression<Func<T, object>> expression)
    {
        result.IsValid.Should()
            .BeFalse();

        result.Errors.Should()
            .NotBeEmpty();

        result.Errors.Should()
            .Contain(x => x.PropertyName == expression.GetMemberName() || x.PropertyName == expression.GetMemberPath());
    }

    public static void ShouldHaveValidationErrorFor(
      this FluentValidation.Results.ValidationResult result,
      Expression<Func<CreateTenantCommand, object>> expression)
    {
        ShouldHaveValidationErrorFor<CreateTenantCommand>(result, expression);
    }
}
