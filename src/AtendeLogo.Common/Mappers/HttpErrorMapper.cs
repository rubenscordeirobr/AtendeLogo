using System.ComponentModel.DataAnnotations;
using System.Net;
using AtendeLogo.Common.Enums;
using AtendeLogo.Common.Exceptions;

namespace AtendeLogo.Common.Mappers;

public static class HttpErrorMapper
{
    public static HttpStatusCode MapErrorToHttpStatusCode(Error error)
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

    public static Error MapHttpStatusCodeToError(
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

    public static Error MapExceptionToError(
        Exception exception,
         string code)
    {
        Guard.NotNull(exception);

        var message = exception.GetNestedMessage();
        return exception switch
        {
            UnauthorizedSecurityException
                => new UnauthorizedError(code, message),
            TaskCanceledException
                => new AbortedError(code, message),
            OperationCanceledException operationCanceledException 
                => new OperationCanceledError(operationCanceledException, code, message),
            ValidationException 
                => new ValidationError(code, message),
            _ => new InternalServerError(exception, code, message)
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

