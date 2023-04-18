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
        var sourceFile = new FileInfo("/MockedSourceFile.md");
        const string existingImageFile = "image1.png";
        const string missingImageFile = "image2.png";

        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(sourceFile.FullName, new MockFileData($"![[{existingImageFile}]] ![[{missingImageFile}]]"));
        fileSystemMock.AddFile($"/{existingImageFile}", new MockFileData(""));

        Assert.True(fileSystemMock.FileExists($"/{existingImageFile}"));
        Assert.False(fileSystemMock.FileExists($"/{missingImageFile}"));

        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var loggerMock = new Mock<ILogger<App>>();
        var fileMoverMock = new Mock<IFileMover>();

        new App(loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, targetDir);

        fileMoverMock.Verify(mover => mover.Move(It.IsAny<FileInfo>(), It.IsAny<DirectoryInfo>()), Times.Never);
    }

    [Fact]
    public void SourceDirDiffersFromMarkdownDir()
    {
        var sourceFile = new FileInfo("/MockedSourceFile.md");
        var imageFile = new FileInfo("/MockedSourceDir/image1.png");

        var fileSystemMock = new MockFileSystem();
        fileSystemMock.AddFile(sourceFile.FullName, new MockFileData($"![[{imageFile.Name}]]"));
        fileSystemMock.AddFile(imageFile.FullName, new MockFileData(""));

        Assert.True(fileSystemMock.FileExists(imageFile.FullName));

        var sourceDir = new DirectoryInfo("/MockedSourceDir");
        var targetDir = new DirectoryInfo("/MockedTargetDir");
        var loggerMock = new Mock<ILogger<App>>();
        var fileMoverMock = new Mock<IFileMover>();

        new App(loggerMock.Object, fileSystemMock, fileMoverMock.Object).Run(sourceFile, sourceDir, targetDir);

        fileMoverMock.Verify(mover => mover.Move(
            It.Is<FileInfo>(f => f.FullName == imageFile.FullName),
            It.Is<DirectoryInfo>(d => d.FullName == targetDir.FullName)),
            Times.Once);
    }
}
