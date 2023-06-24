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
    /// <param name="sourceDir">Search image files in this directory. If this parameter is not specified, then images are searched in the directory of the markdown file.
    /// </param>
    /// <param name="dryRun">Do not move the files (no action). Equivalent to --no-action.</param>
    /// <param name="noAction">Do not move the files (no action). Equivalent to --dry-run.</param>
    public static void Main(FileInfo file, DirectoryInfo targetDir, DirectoryInfo sourceDir = null, bool dryRun = false, bool noAction = false)
    {
        var serviceProvider = ServiceProviderFactory.CreateServiceProvider(dryRun || noAction);
        var app = serviceProvider.GetRequiredService<App>();
        app.Run(file, sourceDir, targetDir);
        serviceProvider.Dispose(); // Flush log messages
    }
}
