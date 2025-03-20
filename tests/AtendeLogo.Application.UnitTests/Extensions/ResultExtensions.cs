namespace AtendeLogo.Application.UnitTests.Extensions;

public static class ResultExtensions
{
    public static void ShouldBeSuccessful<TResponse>(
        this Result<TResponse> result)
        where TResponse : notnull
    {
        result.IsSuccess
            .Should()
            .BeTrue($"Should be successful, but get error {result.Error?.Message}");

        result.IsFailure
            .Should()
            .BeFalse($"Should not be failure, but get error {result.Error?.Message}");

        result.Error
            .Should()
            .BeNull($"Should not have error, but get error {result.Error?.Message}");

        result.Value
            .Should()
            .NotBeNull($"Should have value, but get error {result.Error?.Message}");

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
            .BeTrue($"Should be failure, but get value {result.Value}");

        result.IsSuccess
            .Should()
            .BeFalse($"Should not be successful, but get value {result.Value}");

        result.Error
            .Should()
            .NotBeNull($"Should have error, but get value {result.Value}");

        result.Error
            .Should()
            .BeOfType<TError>($"Should be {typeof(TError).Name} error, but get {result.Error?.GetType().Name}");
    }
}

