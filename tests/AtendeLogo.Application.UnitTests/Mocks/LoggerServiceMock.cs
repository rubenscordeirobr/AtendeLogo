using Microsoft.Extensions.Logging;

namespace AtendeLogo.Application.UnitTests.Mocks;

public class LoggerServiceMock<T> : ILogger<T>
{
    public LoggerServiceMock()
    {

    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {

    }
    public IDisposable BeginScope<TState>(TState state)
    {
        return null!;
    }
}
