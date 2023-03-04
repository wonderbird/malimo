using System.IO;
using Microsoft.Extensions.Logging;

namespace MarkdownLinkedImagesMover;

internal class App
{
    private ILogger<App> Logger { get; }

    public App(ILogger<App> logger) => Logger = logger;

    public void Run(FileInfo file, DirectoryInfo targetDir)
    {
        var fileContent = File.ReadAllText(file.FullName);

        Logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName);
        Logger.LogInformation("File '{@SourceFile}' contains", file.FullName);

        var images = MarkdownParser.ParseLinkedImages(fileContent);
        foreach (var image in images)
        {
            Logger.LogInformation("- '{@ImageFile}'", image);

            var sourceFile = new FileInfo(Path.Combine(file.DirectoryName?? "", image));
            FileMover.Move(sourceFile, targetDir);
        }
    }
}
