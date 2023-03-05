using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MarkdownLinkedImagesMover;

internal class App
{
    private ILogger<App> Logger { get; }

    public App(ILogger<App> logger) => Logger = logger;

    public void Run(FileInfo markdownFile, DirectoryInfo targetDir)
    {
        var fileContent = File.ReadAllText(markdownFile.FullName);
        var imageNames = MarkdownParser.ParseLinkedImages(fileContent).ToList();

        Logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName);
        Logger.LogInformation("File '{@SourceFile}' contains", markdownFile.FullName);
        imageNames.ToList().ForEach(imageName => Logger.LogInformation("- '{@ImageFile}'", imageName));

        imageNames
            .Select(imageName => new FileInfo(Path.Combine(markdownFile.DirectoryName?? "", imageName)))
            .ToList()
            .ForEach(sourceFile => FileMover.Move(sourceFile, targetDir));
    }
}
