namespace AtendeLogo.UseCases.Abstractions;

public interface IApiService : ICommunicationService
{
    string GetVersion();
}

