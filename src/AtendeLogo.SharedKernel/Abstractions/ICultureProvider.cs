namespace AtendeLogo.Shared.Abstractions;

public interface ICultureProvider
{
    Culture Culture { get; }

    Currency Currency { get; }

    Country  Country { get; }
}
