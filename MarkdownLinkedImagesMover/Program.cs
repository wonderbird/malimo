using System.IO;
using System.IO.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace MarkdownLinkedImagesMover;

/// <summary>
/// Program entry point.
/// </summary>
public static class Program
{
    /// <summary>
    /// Move all images referred to by the given markdown file to a folder.
    /// </summary>
    /// <param name="file">Markdown file containing the references to image files which shall be moved</param>
    /// <param name="targetDir">Move files to this target folder</param>
    public static void Main(FileInfo file, DirectoryInfo targetDir)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var app = serviceProvider.GetRequiredService<App>();
        app.Run(file, targetDir);
    }

    private static void ConfigureServices(IServiceCollection serviceCollection) =>
        serviceCollection
            .AddLoggingToConsoleAndDebug()
            .AddTransient<IFileSystem, FileSystem>()
            .AddTransient<IFileMover, FileMover>()
            .AddSingleton<App>();

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
}
