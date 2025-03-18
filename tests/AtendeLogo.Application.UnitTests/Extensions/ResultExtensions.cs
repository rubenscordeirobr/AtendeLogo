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

        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<TResponse>();
    }

    public static void ShouldBeFailure<TError>(
        this IResultValue result)
        where TError : Error
    {
        result.IsFailure
            .Should()
            .BeTrue();

        result.IsSuccess
            .Should()
            .BeFalse();

        result.Error
            .Should()
            .NotBeNull();

        result.Error
            .Should()
            .BeOfType<TError>();
    }
}

