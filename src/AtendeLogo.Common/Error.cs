using System.Net;

namespace AtendeLogo.Common;

public abstract record Error
{
    public string Code { get; }
    public string Message { get; }
    public object[] Arguments { get; }
    public string FormattedMessage
        => GetFormattedMessageInternal();

    public Error(string code, string message, params object[] arguments)
    {
        Guard.NotNullOrWhiteSpace(code, nameof(code));
        Guard.NotNullOrWhiteSpace(message, nameof(message));

        Code = code;
        Message = message;
        Arguments = arguments ?? [];
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

    private string GetFormattedMessageInternal()
    {
        if (Arguments is null || Arguments.Length == 0)
            return Message;

        try
        {
            return string.Format(Message, Arguments);
        }
        catch(FormatException)
        {
            return Message;
        }
    }
}

public record BadRequestError(string Code, 
    string Message, 
    params object[] Arguments) 
    : Error(Code, Message, Arguments);

public record ValidationError(
    string Code,
    string Message, 
    params object[] Arguments) 
    : Error(Code, Message, Arguments);

public record NotFoundError(
    string Code,
    string Message, 
    params object[] Arguments)
    : Error(Code, Message, Arguments);

public record InvalidOperationError(
    string Code,
    string Message,
    params object[] Arguments)
    : Error(Code, Message, Arguments);

public record UnauthorizedError(
    string Code, 
    string Message, 
    params object[] Arguments) 
    : Error(Code, Message, Arguments);

public record DomainEventError(
    string Code, 
    string Message, 
    params object[] Arguments) 
    : Error(Code, Message, Arguments);

public record InternalError(
    Exception Exception,
    string Code,
    string Message,
    params object[] Arguments)
    : Error(Code, Message, Arguments);

public record DatabaseError(
    Exception Exception, 
    string Code,
    string Message,
    params object[] Arguments) 
    : Error(Code, Message, Arguments);

public record OperationCanceledError(
    OperationCanceledException Exception,
    string Code, 
    string Message,
    params object[] Arguments)
    : Error(Code, Message, Arguments);
