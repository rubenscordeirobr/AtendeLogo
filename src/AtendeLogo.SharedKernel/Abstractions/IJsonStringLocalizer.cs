namespace AtendeLogo.Shared.Abstractions;

public interface IJsonStringLocalizer
{
    string this[string localizationKey, string defaultValue, params object[] args] { get; }

}

public interface IJsonStringLocalizer<in T> : IJsonStringLocalizer
{
    string this[T enumValue] { get; }
}

