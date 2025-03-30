namespace AtendeLogo.UseCases.Contracts;

public interface IApiService : ICommunicationService
{
    string GetVersion();
}

