namespace AtendeLogo.Shared.Abstractions;

public interface ICultureProvider
{
    Language Language { get; }
    Culture Culture { get; }

    Currency Currency { get; }

    Country  Country { get; }
}
