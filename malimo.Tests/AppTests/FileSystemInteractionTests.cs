using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace malimo.Tests.AppTests;

public class FileSystemInteractionTests
{
    [Fact]
    public void LastImageDoesNotExist()
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
}