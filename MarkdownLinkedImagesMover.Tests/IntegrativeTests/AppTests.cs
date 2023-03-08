using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarkdownLinkedImagesMover.Tests.IntegrativeTests;

public sealed class AppTests
{
    // TODO: BUG: Test what happens if a file contains two images in the same line!
    [Fact]
    public void ProcessTestfile()
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
