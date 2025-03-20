using Microsoft.Extensions.Logging;

namespace AtendeLogo.TestCommon.Mocks;

public class LoggerServiceMock<T> : ILogger<T>
{

    public LoggerServiceMock()
    {

    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return false;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        // Do nothing

    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }
}
