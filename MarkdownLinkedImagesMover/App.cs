using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MarkdownLinkedImagesMover;

internal class App
{
    private ILogger<App> Logger { get; }

    public App(ILogger<App> logger) => Logger = logger;

    // TODO: Fix static code analysis warnings
#pragma warning disable CA1822
#pragma warning disable CA1848
#pragma warning disable CA2254
    public void Run(FileInfo file, DirectoryInfo targetDir)
    {
        var fileContent = File.ReadAllText(file.FullName);

        Logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName);
        Logger.LogInformation("File '{@SourceFile}' contains", file.FullName);

        var images = MarkdownParser.ParseLinkedImages(fileContent);
        foreach (var image in images)
        {
            Logger.LogInformation("- '{@ImageFile}'", image.Name);
        }
    }
#pragma warning disable CA2254
#pragma warning restore CA1848
#pragma warning restore CA1822
}