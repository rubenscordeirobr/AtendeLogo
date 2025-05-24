namespace AtendeLogo.Shared.Abstractions;

public interface ICultureProvider
{
    Culture Culture { get; }
    Language Language { get; }
}
