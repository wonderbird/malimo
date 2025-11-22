using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace malimo;

internal class App
{
    private readonly ILogger<App> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly IFileMover _fileMover;
    private readonly MarkdownParser _markdownParser;

    public App(ILogger<App> logger, IFileSystem fileSystem, IFileMover fileMover)
    {
        _logger = logger;
        _fileSystem = fileSystem;
        _fileMover = fileMover;
        _markdownParser = new MarkdownParser(fileSystem);
    }

    public void Run(FileInfo markdownFile, DirectoryInfo sourceDir, DirectoryInfo targetDir)
    {
        LogProcessIdForIntegrationTests();

        if (HasInvalidArguments(markdownFile, targetDir))
        {
            return;
        }

        var imageNames = _markdownParser.ParseLinkedImages(markdownFile);

        LogMarkdownFileAnalysisResults(markdownFile, targetDir, imageNames);

        var images = ConstructImagePaths(markdownFile, sourceDir, imageNames);

        MoveImagesToTargetDir(targetDir, images);

        _logger.LogDebug("malimo has completed");
    }

    private void LogProcessIdForIntegrationTests()
    {
        _logger.LogDebug("Process ID {@ProcessId}", Environment.ProcessId);
    }

    private bool HasInvalidArguments(FileInfo markdownFile, DirectoryInfo targetDir)
    {
        var isValid = true;

        if (markdownFile == null)
        {
            _logger.LogError("Missing --file option");
            isValid = false;
        }

        if (targetDir == null)
        {
            _logger.LogError("Missing --target-dir option");
            isValid = false;
        }

        return !isValid;
    }

    private void LogMarkdownFileAnalysisResults(
        FileInfo markdownFile,
        DirectoryInfo targetDir,
        List<string> imageNames
    )
    {
        _logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName);
        _logger.LogInformation("File '{@SourceFile}' contains", markdownFile.FullName);
        imageNames.ForEach(imageName => _logger.LogInformation("- '{@ImageFile}'", imageName));
    }

    private static List<FileInfo> ConstructImagePaths(
        FileInfo markdownFile,
        DirectoryInfo sourceDir,
        List<string> imageNames
    )
    {
        var baseDirectory = sourceDir?.FullName ?? markdownFile.DirectoryName ?? "";
        return imageNames.Select(imageName => new FileInfo(Path.Combine(baseDirectory, imageName))).ToList();
    }

    private void MoveImagesToTargetDir(DirectoryInfo targetDir, List<FileInfo> images)
    {
        if (ContainsMissingFiles(images))
        {
            return;
        }

        images.ForEach(image => _fileMover.Move(image, targetDir));
    }

    private bool ContainsMissingFiles(List<FileInfo> images)
    {
        var missingFiles = images.Where(file => !_fileSystem.File.Exists(file.FullName)).ToList();
        LogMissingFiles(missingFiles);
        return missingFiles.Count == 0;
    }

    private void LogMissingFiles(List<FileInfo> missingFiles) =>
        missingFiles.ForEach(file => _logger.LogError("File '{@ImageFile}' does not exist", file.FullName));
}
