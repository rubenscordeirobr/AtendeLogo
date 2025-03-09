using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Infrastructure.Services;

public class JsonStringLocalizer<T> : IJsonStringLocalizer<T>
{
    public string this[string name, string defaultValue, params object[] args]
        => StringFormatUtils.Format(defaultValue, args);
}
