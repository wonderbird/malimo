using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace malimo;

/// <summary>
/// Program entry point.
/// </summary>
public static class Program
{
    /// <summary>
    /// Move all images used by a markdown file to a folder.
    /// </summary>
    /// <param name="file">Markdown file containing the references to image files which shall be moved</param>
    /// <param name="targetDir">Move files to this target folder</param>
    /// <param name="dryRun">Do not move the files (no action)</param>
    public static void Main(FileInfo file, DirectoryInfo targetDir, bool dryRun = false)
    {
        var serviceProvider = ServiceProviderFactory.CreateServiceProvider(dryRun);
        var app = serviceProvider.GetRequiredService<App>();
        app.Run(file, targetDir);
        serviceProvider.Dispose(); // Flush log messages
    }
}
