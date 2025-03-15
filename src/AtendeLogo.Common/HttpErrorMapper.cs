using System.Net;
using AtendeLogo.Common.Enums;

namespace AtendeLogo.Common;

public static class HttpErrorMapper
{
    public static HttpStatusCode GetHttpStatusCode(Error error)
    {
        return error switch
        {
            BadRequestError => HttpStatusCode.BadRequest,
            UnauthorizedError => HttpStatusCode.Unauthorized,
            ValidationError => HttpStatusCode.UnprocessableContent,
            NotFoundError => HttpStatusCode.NotFound,
            DomainEventError => HttpStatusCode.Conflict,
            NotImplementedError => HttpStatusCode.NotImplemented,
            AbortedError => (HttpStatusCode)499,
            _ => HttpStatusCode.InternalServerError
        };
    }

    public static Error GetErrorFromStatus(
        HttpStatusCode statusCode,
        string code,
        string message)
    {
        return statusCode switch
        {
            HttpStatusCode.InternalServerError
                => new InternalServerError(null!, code, message),
            HttpStatusCode.BadRequest
                => new BadRequestError(code, message),
            HttpStatusCode.NotFound
                => new NotFoundError(code, message),
            HttpStatusCode.UnprocessableContent
                => new ValidationError(code, message),
            HttpStatusCode.Conflict
                => new DomainEventError(code, message),
            _ => ResolverExtendedStatus(statusCode, code, message)
        };
    }

    private static Error ResolverExtendedStatus(
        HttpStatusCode statusCode,
        string code,
        string message)
    {
        var extendStatus = (ExtendedHttpStatusCode)statusCode;
        if (Enum.IsDefined(extendStatus))
        {
            return extendStatus switch
            {
                ExtendedHttpStatusCode.RequestAborted => new AbortedError(code, message),
                _ => new NotImplementedError($"ExtendedHttpStatusCode.NotImplemented.{code}", message)
            };
        }

        return new NotImplementedError($"HttpStatusCode.NotImplemented.{code}", message);
    }
}

