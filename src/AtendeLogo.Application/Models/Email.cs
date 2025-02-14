namespace AtendeLogo.Application.Models;

public record Email
{
    public required string To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public required string From { get; init; }
    public string? FromName { get; init; }

    public Email(string to, string subject, string body, string from, string fromName)
    {
        To = to;
        Subject = subject;
        Body = body;
        From = from;
        FromName = fromName;
    }
}
