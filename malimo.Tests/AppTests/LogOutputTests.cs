using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace malimo.Tests.AppTests;

public sealed class LogOutputTests
{
    private readonly Mock<ILogger<App>> _loggerMock = new();

    [Fact]
    public void MissingFileArgument()
    {
        var dir = new DirectoryInfo("arbitrary string");

        new App(_loggerMock.Object, null, null).Run(null, dir);

        _loggerMock.VerifyLog(logger => logger.LogError("ERROR: Missing --file option"));
    }

    [Fact]
    public void MissingTargetDirArgument()
    {
        const string sourceFileFullName = "/MockedSourceFile.md";
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("") }
            }
        );
        var sourceFile = new FileInfo(sourceFileFullName);

        new App(_loggerMock.Object, fileSystemMock, null).Run(sourceFile, null);

        _loggerMock.VerifyLog(logger => logger.LogError("ERROR: Missing --target-dir option"));
    }

    [Fact]
    public void SomeImagesDoNotExist()
    {
        const string sourceFileFullName = "/MockedSourceFile.md";
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("![[image1.png]] ![[image2.png]] ![[image3.png]]") },
                // /image1.png does not exist
                { "/image2.png", new MockFileData("") }
                // /image3.png does not exist
            }
        );

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var sourceFile = new FileInfo(sourceFileFullName);
        var fileMoverMock = new Mock<IFileMover>();

        new App(_loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

        _loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image1.png"));
        _loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image3.png"));
    }

    [Fact]
    public void AllImagesExist()
    {
        const string sourceFileFullName = "/MockedSourceFile.md";
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("![[noun-starship-3799189.png]]\n![[noun-island-1479438.png]]") }
            }
        );

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var sourceFile = new FileInfo(sourceFileFullName);
        var fileMoverMock = new Mock<IFileMover>();

        new App(_loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

        _loggerMock.VerifyLog(logger => logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName));
        _loggerMock.VerifyLog(logger => logger.LogInformation("File '{@SourceFile}' contains", sourceFile.FullName));
        _loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-starship-3799189.png"));
        _loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-island-1479438.png"));
    }
}
