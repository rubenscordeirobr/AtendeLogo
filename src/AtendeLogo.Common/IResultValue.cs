using System.Diagnostics.CodeAnalysis;

namespace AtendeLogo.Common;

public interface IResultValue
{
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    bool IsSuccess { get; }
    Error? Error { get; }

    [MemberNotNullWhen(true, nameof(IsSuccess))]
    object? Value { get; }

    [MemberNotNullWhen(true, nameof(Error))]
    bool IsFailure => !IsSuccess;
}
