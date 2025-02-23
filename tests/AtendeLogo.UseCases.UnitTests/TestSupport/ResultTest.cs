using System;
using System.Net;
using FluentAssertions;
using Xunit;
using AtendeLogo.Common;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.UseCases.UnitTests.TestSupport;

// Existing code
public class ResultTest<T>
{
    private Result<IResponse> _result { get; }

    public Error? Error => _result.Error;

    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Value))]
    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => _result.IsSuccess;

    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(Error))]
    public bool IsFailure => _result.IsFailure;

    [System.Diagnostics.CodeAnalysis.MemberNotNullWhen(true, nameof(IsSuccess))]
    public IResponse? Value => _result.Value;

    public ResultTest(Result<IResponse> result)
    {
        _result = result;
    }
}
