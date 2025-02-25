using System.Diagnostics.CodeAnalysis;
using System.Net;
using AtendeLogo.Presentation.Common.Enums;

namespace AtendeLogo.Presentation.Common;

public record ResponseResult
{
    [MemberNotNullWhen(false, nameof(ErroResult))]
    public bool IsSuccess { get; }
    public int StatusCode { get; }
    public object? Response { get; }

    public ErroResult? ErroResult { get; }

    private ResponseResult(HttpStatusCode statusCode, object response)
    {
        Response = response;
        StatusCode = (int)statusCode;
        IsSuccess = true;
    }

    private ResponseResult(HttpStatusCode statusCode, string errorCode, string errorMessage)
    {
        ErroResult = new ErroResult(errorCode, errorMessage);
        StatusCode = (int)statusCode;
        IsSuccess = false;
    }
    private ResponseResult(ExtendedHttpStatusCode statusCode, string errorCode, string errorMessage)
    {
        ErroResult = new ErroResult(errorCode, errorMessage);
        StatusCode = (int)statusCode;
        IsSuccess = false;
    }
    public static ResponseResult Ok(object response)
        => new ResponseResult(HttpStatusCode.OK, response);

    public static ResponseResult SuccessWithStatus(
        HttpStatusCode statusCode,
        object response)
        => new ResponseResult(statusCode, response);

    public static ResponseResult Error(
        HttpStatusCode statusCode,
        string errorCode,
        string errorMessage)
        => new ResponseResult(statusCode, errorCode, errorMessage);

    public static ResponseResult Error(
        ExtendedHttpStatusCode statusCode,
        string errorCode,
        string errorMessage)
        => new ResponseResult(statusCode, errorCode, errorMessage);

    public static ResponseResult Error(Error error)
        => new ResponseResult(error.StatusCode, error.Code, error.FormattedMessage);
}

public record ErroResult(
    string Code,
    string Message);
