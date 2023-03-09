using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace MarkdownLinkedImagesMover;

internal static class ServiceProviderFactory
{
    public static ServiceProvider CreateServiceProvider(bool noAction)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection, noAction);
        return serviceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection serviceCollection, bool noAction)
    {
        serviceCollection
            .AddLoggingToConsoleAndDebug()
            .AddTransient<IFileSystem, FileSystem>()
            .AddFileMover(noAction)
            .AddSingleton<App>();
    }

    private static IServiceCollection AddLoggingToConsoleAndDebug(this IServiceCollection serviceCollection) =>
        serviceCollection.AddLogging(configure =>
        {
            configure.SetMinimumLevel(LogLevel.Debug);
            configure.AddDebug();
            configure.AddConsole(options =>
            {
                // Show all log messages immediately
                options.MaxQueueLength = 1;
                options.FormatterName = nameof(SingleLineConsoleFormatter);
            });
            configure.AddConsoleFormatter<SingleLineConsoleFormatter, ConsoleFormatterOptions>();
        });

    private static IServiceCollection AddFileMover(this IServiceCollection serviceCollection, bool noAction)
    {
        if (noAction)
        {
            serviceCollection.AddTransient<IFileMover, NoAction>();
        }
        else
        {
            serviceCollection.AddTransient<IFileMover, FileMover>();
        }

        return serviceCollection;
    }
}