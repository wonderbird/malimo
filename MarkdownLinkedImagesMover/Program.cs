using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace MarkdownLinkedImagesMover;

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
    /// <param name="noAction">Do not move the files (dry run)</param>
    public static void Main(FileInfo file, DirectoryInfo targetDir, bool noAction = false)
    {
        var serviceProvider = ServiceProviderFactory.CreateServiceProvider(noAction);
        var app = serviceProvider.GetRequiredService<App>();
        app.Run(file, targetDir);
    }
}