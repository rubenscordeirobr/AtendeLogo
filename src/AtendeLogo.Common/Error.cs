using System.Net;

namespace AtendeLogo.Common;

public abstract record Error
{
    public string Code { get; }
    public string Message { get; }
    public Exception? Exception { get; }
    public ErrorResponse ErrorResponse
        => new ErrorResponse(Code, Message);

    protected Error(string code, string message)
    {
        Guard.NotNullOrWhiteSpace(code, nameof(code));
        Guard.NotNullOrWhiteSpace(message, nameof(message));

        Code = code;
        Message = message;
    }
    public Error(Exception? exception, string code, string message)
    {
        Guard.NotNullOrWhiteSpace(code, nameof(code));
        Guard.NotNullOrWhiteSpace(message, nameof(message));

        Code = code;
        Message = message;
        Exception = exception;
    }

    public sealed override string ToString()
        => $"{Code}: {Message}";

    public static implicit operator string(Error error)
        => error.ToString();

    public virtual HttpStatusCode StatusCode
        => HttpErrorMapper.GetHttpStatusCode(this);
}

public record BadRequestError(string Code,
    string Message)
    : Error(Code, Message);

public record ValidationError(
    string Code,
    string Message)
    : Error(Code, Message);

public record CommandValidatorNotFoundError(
    Exception Exception,
    string Code,
    string Message)
    : Error(Exception, Code, Message);

public record NotFoundError(
    string code,
    string message)
    : Error(code, message);

public record InvalidOperationError(
    string Code,
    string Message)
    : Error(Code, Message);

public record UnauthorizedError(
    string Code,
    string Message)
    : Error(Code, Message);

public record DomainEventError(
    string Code,
    string Message)
    : Error(Code, Message);

public record InternalServerError(
    Exception? Exception,
    string Code,
    string Message)
    : Error(Exception, Code, Message);

public record DatabaseError(
    Exception Exception,
    string Code,
    string Message)
    : Error(Exception, Code, Message);

public record OperationCanceledError(
    Exception? Exception,
    string Code,
    string Message)
    : Error(Exception, Code, Message);

public record UnknownError(
    Exception Exception,
    string Code,
    string Message)
    : Error(Exception, Code, Message);

public partial record DeserializationError(
    Exception Exception,
    string Code,
    string Message)
    : Error(Exception, Code, Message);

public record NotImplementedError(
    string Code,
    string Message)
    : Error(Code, Message);

public record AbortedError(
    string Code,
    string Message)
    : Error(Code, Message);

// Used specifically for serialization to API responses
public record ErrorResponse(
    string Code,
    string Message);

public partial record DeserializationError
{
    public static DeserializationError Create<T>(
        Exception ex,
        string code,
        string json)
    {
        json = json.SafeTrim(1024, "[truncated]");
        return new DeserializationError(ex,
               code,
               $"An error occurred while deserializing. " +
               $"Type : {typeof(T).Name}" +
               $"Message: {ex.Message}\r\n" +
               $"Json: {json}");
    }
}
