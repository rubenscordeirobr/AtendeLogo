using System.Linq.Expressions;
using AtendeLogo.Common;
using AtendeLogo.UseCases.Identities.Tenants.Commands;

namespace AtendeLogo.Application.UnitTests.Extensions;

public static class ResultExtensions
{
    public static void ShouldBeSuccessful<TResponse>(
        this Result<TResponse> result)
        where TResponse : notnull
    {
        result.IsSuccess
            .Should()
            .BeTrue();

        result.IsFailure
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .BeNull();
    }

    public static void ShouldHaveValidationErrorFor<TRequest, TResponse>(
        this Result<TResponse> result,
        Expression<Func<TRequest, object>> propertyExpression)
        where TResponse : notnull
    {
        result.Error
            .Should()
            .NotBeNull();

        result.IsFailure
            .Should()
            .BeTrue();

        result.IsSuccess
            .Should()
            .BeFalse();

        var errorCode = ErrorCodeFactory.CreateInvalidCodeFor(propertyExpression);
        result.Error.Should()
            .BeOfType<ValidationError>()
            .Subject.Code
            .Should()
            .Be(errorCode);

    }

    public static void ShouldHaveValidationErrorFor(
        this Result<CreateTenantResponse> result,
        Expression<Func<CreateTenantCommand, object>> propertyExpression)
    {
        result.ShouldHaveValidationErrorFor<CreateTenantCommand, CreateTenantResponse>(
            propertyExpression);
    }
}

