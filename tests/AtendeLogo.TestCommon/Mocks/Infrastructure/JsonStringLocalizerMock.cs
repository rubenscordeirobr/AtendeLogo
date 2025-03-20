using AtendeLogo.Common.Utils;

namespace AtendeLogo.TestCommon.Mocks.Infrastructure;

public class JsonStringLocalizerMock<T> : IJsonStringLocalizer<T>
{
    public string this[string name, string defaultValue, params object[] args]
        => StringFormatUtils.Format(defaultValue, args);
}
