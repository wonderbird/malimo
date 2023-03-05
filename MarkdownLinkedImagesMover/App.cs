using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace MarkdownLinkedImagesMover;

internal class App
{
    private readonly ILogger<App> _logger;

    public App(ILogger<App> logger) => _logger = logger;

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
        _logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName);
        _logger.LogInformation("File '{@SourceFile}' contains", markdownFile.FullName);
        imageNames.ForEach(imageName => _logger.LogInformation("- '{@ImageFile}'", imageName));
    }

    private static void MoveImagesToTargetDir(FileInfo markdownFile, DirectoryInfo targetDir, IEnumerable<string> imageNames) =>
        imageNames
            .Select(imageName => new FileInfo(Path.Combine(markdownFile.DirectoryName ?? "", imageName)))
            .ToList()
            .ForEach(sourceFile => FileMover.Move(sourceFile, targetDir));
}
