using Microsoft.Extensions.Logging;

namespace TestHost;

/// <summary>
///     Allow use of ILogger<> at dependency registration level
///     Required to bypass issue with Azure Function DI not registering
///     logging dependencies soon enough
/// </summary>
public class LogAdapter<T> : ILogger<T>
{
    private readonly Lazy<ILogger> _logger;

    public LogAdapter(ILoggerProvider provider) =>
        _logger = new Lazy<ILogger>(() => provider.CreateLogger(typeof(T).Name));

    public IDisposable BeginScope<TState>(TState state) => _logger.Value.BeginScope(state);

    public bool IsEnabled(LogLevel logLevel) => _logger.Value.IsEnabled(logLevel);

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter
    ) => _logger.Value.Log(logLevel, eventId, state, exception, formatter);
}