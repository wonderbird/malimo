using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace malimo;

internal sealed class SingleLineConsoleFormatter : ConsoleFormatter
{
    public SingleLineConsoleFormatter(IOptionsMonitor<ConsoleFormatterOptions> _)
        : base(nameof(SingleLineConsoleFormatter)) { }

    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider scopeProvider,
        TextWriter textWriter
    )
    {
        var message = logEntry.Formatter.Invoke(logEntry.State, logEntry.Exception);
        textWriter.WriteLine($"{logEntry.LogLevel}: {message}");
    }
}
