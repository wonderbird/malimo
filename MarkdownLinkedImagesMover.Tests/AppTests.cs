using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarkdownLinkedImagesMover.Tests;

public sealed class AppTests
{
    [Fact]
    public void GivenLastImageDoesNotExist_DoesNotMoveAnyFile()
    {
        const string sourceFileFullName = "/MockedSourceFile.md";
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("![[image1.png]] ![[image2.png]]") },
                { "/image1.png", new MockFileData("") }
            }
        );

        Assert.False(fileSystemMock.FileExists("/image.png"));

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var sourceFile = new FileInfo(sourceFileFullName);
        var loggerMock = new Mock<ILogger<App>>();
        var fileMoverMock = new Mock<IFileMover>();
        new App(loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

// TODO: Verify that an error has been reported in a separate test - loggerMock.VerifyLog(logger => logger.LogError("File '{@ImageFile}' does not exist", "/image.png"));
        fileMoverMock.Verify(mover => mover.Move(It.IsAny<FileInfo>(), It.IsAny<DirectoryInfo>()), Times.Never);
    }

    [Fact]
    public void GivenAllImagesExist_ProducesCorrectLogs()
    {
        const string sourceFileFullName = "/MockedSourceFile.md";
        var targetDir = new DirectoryInfo("/MockedTargetDir");

        var loggerMock = new Mock<ILogger<App>>();

        var sourceFile = new FileInfo(sourceFileFullName);
        var fileSystemMock = new MockFileSystem(
            new Dictionary<string, MockFileData>
            {
                { sourceFileFullName, new MockFileData("![[noun-starship-3799189.png]]\n![[noun-island-1479438.png]]") }
            }
        );

        new App(loggerMock.Object, fileSystemMock, Mock.Of<IFileMover>()).Run(sourceFile, targetDir);

        loggerMock.VerifyLog(logger => logger.LogInformation("Target folder: '{@TargetFolder}'", targetDir.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("File '{@SourceFile}' contains", sourceFile.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-starship-3799189.png"));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-island-1479438.png"));
    }
}
