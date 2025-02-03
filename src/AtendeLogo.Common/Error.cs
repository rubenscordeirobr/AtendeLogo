
using System.Diagnostics.CodeAnalysis;

namespace AtendeLogo.Common;

[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
public abstract record Error 
{
    public string Code { get;  }

    public string Message { get; }

    public Error(string code, string message)
    {
        Guard.NotNullOrWhiteSpace(code, nameof(code));
        Guard.NotNullOrWhiteSpace(message, nameof(message));
       
        Code = code;
        Message = message;
    }
     
    public override string ToString()
        => $"{Code}: {Message}";

    public static implicit operator string(Error error)
        => error.ToString();

    public int StatusCode => this switch
    {
        BadRequestError => 400,
        UnauthorizedError => 401,
        ValidationError => 422,
        NotFoundError => 404,
        InternalError => 500,
        _ => throw new NotSupportedException($"Error type {GetType().Name} is not supported")
    };
}

public record BadRequestError(string Code, string Message) : Error(Code, Message);
public record ValidationError(string Code, string Message) : Error(Code, Message);
public record NotFoundError(string Code, string Message) : Error(Code, Message);
public record InternalError(string Code, string Message) : Error(Code, Message);
public record UnauthorizedError(string Code, string Message) : Error(Code, Message);
