using AtendeLogo.Common.Utils;
using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

public class JsonStringLocalizerMock<T> : IJsonStringLocalizer<T>
{
    public string this[string name, string defaultValue, params object[] args]
        => StringFormatUtils.Format(defaultValue, args);
}
