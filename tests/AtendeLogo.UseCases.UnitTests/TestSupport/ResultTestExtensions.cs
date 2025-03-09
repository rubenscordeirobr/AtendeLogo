using System.Linq.Expressions;

namespace AtendeLogo.UseCases.UnitTests.TestSupport;

public static class ResultTestExtensions
{
    public static void ShouldBeSuccessful<T>(
        this ResultTest<T> result)
    {
        result.Error.Should()
            .BeNull();

        result.IsFailure
            .Should()
            .BeFalse();

        result.IsSuccess
            .Should()
            .BeTrue();
    }

    public static void ShouldNotBeSuccessful<T>(
       this ResultTest<T> result)
    {
        result.Error.Should()
            .NotBeNull();

        result.IsFailure
            .Should()
            .BeTrue();

        result.IsSuccess
            .Should()
            .BeFalse();
    }

    public static void ShouldHaveValidationErrorFor<T>(
        this ResultTest<T> result,
        Expression<Func<T, object>> propertyExpression)
    {
        result.Error.Should()
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
}
