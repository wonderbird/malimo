using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace malimo.Tests;

public sealed class AppTests
{
    [Fact]
    public void WhenLastImageDoesNotExist_ThenDoNotMoveAnyFile()
    {
        const string sourceFileFullName = "/MockedSourceFile.md";
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("![[image1.png]] ![[image2.png]]") },
                { "/image1.png", new MockFileData("") }
                // /image2.png does not exist
            }
        );

        Assert.False(fileSystemMock.FileExists("/image.png"));

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var sourceFile = new FileInfo(sourceFileFullName);
        var loggerMock = new Mock<ILogger<App>>();
        var fileMoverMock = new Mock<IFileMover>();

        new App(loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

        fileMoverMock.Verify(mover => mover.Move(It.IsAny<FileInfo>(), It.IsAny<DirectoryInfo>()), Times.Never);
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
