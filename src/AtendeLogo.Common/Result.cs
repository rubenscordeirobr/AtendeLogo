using System.Diagnostics.CodeAnalysis;

namespace AtendeLogo.Common;

public class Result<T> where T : notnull
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure 
        => !IsSuccess;


    public Error? Error { get; }

    [MemberNotNullWhen(true, nameof(IsSuccess))]
    public T? Value { get; }

    internal Result(Error error)
    {
        Guard.NotNull(error);

        Error = error;
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

    public static Result<T> Failure<T>(Error error)
         where T : notnull
          => new(error);

    public static Result<T> ValidationFailure<T>(
        string code,
        string message, 
        params object[] arguments)
        where T : notnull
        => new(new ValidationError(code, message, arguments));
   
    public static Result<T> NotFoundFailure<T>(
        string code,
        string message,
        params object[] arguments)
        where T : notnull
        => new(new NotFoundError(code, message, arguments));
}
