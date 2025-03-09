using System.Net;

namespace AtendeLogo.Common;

public abstract record Error
{
    public string Code { get; }
    public string Message { get; }

    public Error(string code, string message)
    {
        Guard.NotNullOrWhiteSpace(code, nameof(code));
        Guard.NotNullOrWhiteSpace(message, nameof(message));

        Code = code;
        Message = message;
    }

    public sealed override string ToString()
        => $"{Code}: {Message}";

    public static implicit operator string(Error error)
        => error.ToString();

    public HttpStatusCode StatusCode => this switch
    {
        BadRequestError => HttpStatusCode.BadRequest,
        UnauthorizedError => HttpStatusCode.Unauthorized,
        ValidationError => HttpStatusCode.UnprocessableContent,
        NotFoundError => HttpStatusCode.NotFound,
        DomainEventError => HttpStatusCode.Conflict,
        InternalError => HttpStatusCode.InternalServerError,
        DatabaseError => HttpStatusCode.InternalServerError,
        OperationCanceledError => HttpStatusCode.InternalServerError,
        InvalidOperationError => HttpStatusCode.InternalServerError,
        _ => throw new NotSupportedException($"Error type {GetType().Name} is not supported")
    };
}

public record BadRequestError(string Code,
    string Message)
    : Error(Code, Message);

public record ValidationError(
    string Code,
    string Message)
    : Error(Code, Message);

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

public record InternalError(
    Exception Exception,
    string Code,
    string Message)
    : Error(Code, Message);

public record DatabaseError(
    Exception Exception,
    string Code,
    string Message)
    : Error(Code, Message);

public record OperationCanceledError : Error
{
    public OperationCanceledException Exception { get; }

    public OperationCanceledError(
        OperationCanceledException exception,
        string Code,
        string Message)
        : base(Code, Message)
    {
        Exception = exception;
    }

    public OperationCanceledError(
        string Code,
        string Message)
        : base(Code, Message)
    {
        Exception = new OperationCanceledException(Message);
    }
}

