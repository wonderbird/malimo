using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace malimo.Tests.AppTests;

public sealed class LogOutputTests
{
    [Fact]
    public void WhenMissingFileArgument_ThenLogError()
    {
        var loggerMock = new Mock<ILogger<App>>();
        var dir = new DirectoryInfo("arbitrary string");

        new App(loggerMock.Object, null, null).Run(null, dir);

        loggerMock.VerifyLog(logger => logger.LogError("ERROR: Missing --file option"));
    }

    [Fact]
    public void WhenMissingTargetDirArgument_ThenLogError()
    {
        var loggerMock = new Mock<ILogger<App>>();
        const string sourceFileFullName = "/MockedSourceFile.md";
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("") }
            }
        );
        var sourceFile = new FileInfo(sourceFileFullName);

        new App(loggerMock.Object, fileSystemMock, null).Run(sourceFile, null);

        loggerMock.VerifyLog(logger => logger.LogError("ERROR: Missing --target-dir option"));
    }

    [Fact]
    public void WhenImagesDoNotExist_ThenLogMissingFiles()
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
        var loggerMock = new Mock<ILogger<App>>();
        var fileMoverMock = new Mock<IFileMover>();

        new App(loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

        loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image1.png"));
        loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image3.png"));
    }

    [Fact]
    public void WhenAllImagesExist_ThenProduceCorrectLogs()
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
        var loggerMock = new Mock<ILogger<App>>();
        var fileMoverMock = new Mock<IFileMover>();

        new App(loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

        loggerMock.VerifyLog(logger => logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("File '{@SourceFile}' contains", sourceFile.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-starship-3799189.png"));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-island-1479438.png"));
    }
}
