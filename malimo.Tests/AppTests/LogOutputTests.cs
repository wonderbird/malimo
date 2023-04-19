using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace malimo.Tests.AppTests;

public sealed class LogOutputTests
{
    private readonly Mock<ILogger<App>> _loggerMock = new();
    private readonly MockFileSystem _fileSystemMock = new();

    [Fact]
    public void MissingRequiredFileArgument()
    {
        var dir = new DirectoryInfo("arbitrary string");

        new App(_loggerMock.Object, null, null).Run(null, null, dir);

        _loggerMock.VerifyLog(logger => logger.LogError("Missing --file option"));
    }

    [Fact]
    public void MissingRequiredTargetDirArgument()
    {
        var sourceFile = new FileInfo("/MockedSourceFile.md");
        _fileSystemMock.AddFile(sourceFile.FullName, new MockFileData(""));

        new App(_loggerMock.Object, _fileSystemMock, null).Run(sourceFile, null, null);

        _loggerMock.VerifyLog(logger => logger.LogError("Missing --target-dir option"));
    }

    [Fact]
    public void MissingRequiredFileAndTargetDirArguments()
    {
        new App(_loggerMock.Object, null, null).Run(null, null, null);

        _loggerMock.VerifyLog(logger => logger.LogError("Missing --file option"));
        _loggerMock.VerifyLog(logger => logger.LogError("Missing --target-dir option"));
    }

    [Fact]
    public void SomeImagesDoNotExist()
    {
        var sourceFile = new FileInfo("/MockedSourceFile.md");
        _fileSystemMock.AddFile(
            sourceFile.FullName,
            new MockFileData("![[image1.png]] ![[image2.png]] ![[image3.png]]")
        );
        _fileSystemMock.AddFile("/image2.png", new MockFileData(""));

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var fileMoverMock = new Mock<IFileMover>();

        new App(_loggerMock.Object, _fileSystemMock, fileMoverMock.Object).Run(sourceFile, null, targetDir);

        _loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image1.png"));
        _loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image3.png"));
    }

    [Fact]
    public void TwoExistingImages()
    {
        var sourceFile = new FileInfo("/MockedSourceFile.md");
        _fileSystemMock.AddFile(
            sourceFile.FullName,
            new MockFileData("![[noun-starship-3799189.png]]\n![[noun-island-1479438.png]]")
        );

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var fileMoverMock = new Mock<IFileMover>();

        new App(_loggerMock.Object, _fileSystemMock, fileMoverMock.Object).Run(sourceFile, null, targetDir);

        _loggerMock.VerifyLog(logger => logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName));
        _loggerMock.VerifyLog(logger => logger.LogInformation("File '{@SourceFile}' contains", sourceFile.FullName));
        _loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-starship-3799189.png"));
        _loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-island-1479438.png"));
    }
}
