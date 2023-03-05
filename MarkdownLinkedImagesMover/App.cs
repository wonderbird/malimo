using System.Collections.Generic;
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
        var imageNames = GetImagesFromMarkdownFile(markdownFile);
        LogImageNames(markdownFile, targetDir, imageNames);
        MoveImagesToTargetDir(markdownFile, targetDir, imageNames);
    }

    private static List<string> GetImagesFromMarkdownFile(FileSystemInfo markdownFile)
    {
        var fileContent = File.ReadAllText(markdownFile.FullName);
        return MarkdownParser.ParseLinkedImages(fileContent).ToList();
    }

    private void LogImageNames(FileSystemInfo markdownFile, FileSystemInfo targetDir, List<string> imageNames)
    {
        Logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName);
        Logger.LogInformation("File '{@SourceFile}' contains", markdownFile.FullName);
        imageNames.ForEach(imageName => Logger.LogInformation("- '{@ImageFile}'", imageName));
    }

    private static void MoveImagesToTargetDir(FileInfo markdownFile, DirectoryInfo targetDir, IEnumerable<string> imageNames) =>
        imageNames
            .Select(imageName => new FileInfo(Path.Combine(markdownFile.DirectoryName ?? "", imageName)))
            .ToList()
            .ForEach(sourceFile => FileMover.Move(sourceFile, targetDir));
}
