using Microsoft.Extensions.Logging;

namespace AtendeLogo.Common.Mappers;

public static class ErrorLogLevelMapper
{
    public static LogLevel ResolveErrorLevel(Error error)
    {
        return error switch
        {
            NotFoundError => LogLevel.Information,
            OperationCanceledError => LogLevel.Information,
            // Warnings
            ValidationError => LogLevel.Warning,
            UnauthorizedError => LogLevel.Warning,
            AbortedError => LogLevel.Warning,
            TaskTimeoutError => LogLevel.Warning,
            // Errors
            BadRequestError => LogLevel.Error,
            InvalidOperationError => LogLevel.Error,
            DomainEventError => LogLevel.Error,
            RequestError => LogLevel.Error,
            DeserializationError => LogLevel.Error,
            CreateHttpRequestMessageError => LogLevel.Error,
            UnknownError => LogLevel.Error,
            //Critical
            DatabaseError => LogLevel.Critical,
            CommandValidatorNotFoundError => LogLevel.Critical,
            InternalServerError => LogLevel.Critical,
            NotImplementedError => LogLevel.Critical,
            _ => LogLevel.Error
        };
    }
}
