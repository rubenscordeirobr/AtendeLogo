namespace AtendeLogo.Shared.Contracts;

public interface IJsonStringLocalizer<out T>
{
    string this[string name, string defaultValue, params object[] args] { get; }

}
