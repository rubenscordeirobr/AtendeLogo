
using System.Diagnostics.CodeAnalysis;

namespace AtendeLogo.Domain;

[SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords")]
public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Erro.NullValue", "The value is null");

    public override string ToString()
        => $"{Code}: {Message}";

    public static implicit operator string(Error error)
        => error.ToString();
}
