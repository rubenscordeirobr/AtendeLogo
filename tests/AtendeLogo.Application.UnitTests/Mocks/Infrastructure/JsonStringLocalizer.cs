using AtendeLogo.Shared.Contracts;

namespace AtendeLogo.Application.UnitTests.Mocks.Infrastructure;

internal class JsonStringLocalizer<T> : IJsonStringLocalizer<T>
{
    public string this[string name, string defaultValue] 
        =>  defaultValue;
}
