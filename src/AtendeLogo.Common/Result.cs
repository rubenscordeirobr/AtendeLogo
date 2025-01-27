namespace AtendeLogo.Domain;

public class Result<T> where T : notnull
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }
    public T? Value { get; }

    internal Result(Error? error)
    {
        Error = error ?? throw new ArgumentNullException(nameof(error));
        IsSuccess = false;
    }

    internal Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    public Result<TConvert> AsFailure<TConvert>() where TConvert : notnull
    {
        if (IsSuccess || Error is null)
            throw new InvalidOperationException("Only failed results can be converted");

        return Result.Failure<TConvert>(Error);
    }

    public T GetValue()
    {
        if (!IsSuccess || Value is null)
            throw new InvalidOperationException("Cannot get value from failed result");
        return Value;
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value)
        where T : notnull
        => new(value);

    public static Result<T> Failure<T>(string code, string errorMessage)
        where T : notnull
        => new(new Error(code, errorMessage));
    public static Result<T> Failure<T>(Error error)
        where T : notnull
        => new(error);
}