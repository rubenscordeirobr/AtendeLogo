using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Infrastructure.Services;

public class JsonStringLocalizer<T> : IJsonStringLocalizer<T>
{
    public string this[string name, string defaultValue]
        => defaultValue;
}