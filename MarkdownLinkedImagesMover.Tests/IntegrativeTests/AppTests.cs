using System.IO;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarkdownLinkedImagesMover.Tests.IntegrativeTests;

public sealed class AppTests
{
    [Fact]
    public void ProcessTestfile()
    {
        using var testDir = TestDirectory.Create();

        var loggerMock = new Mock<ILogger<App>>();
        var sourceFile = new FileInfo(Path.Combine(testDir.SourceDir.FullName, "Testfile.md"));

        new App(loggerMock.Object).Run(sourceFile, testDir.TargetDir);

        loggerMock.VerifyLog(
            logger => logger.LogInformation("Target folder: '{@TargetFolder}'", testDir.TargetDir.FullName)
        );
        loggerMock.VerifyLog(logger => logger.LogInformation("File '{@SourceFile}' contains", sourceFile.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-starship-3799189.png"));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-island-1479438.png"));
    }
}
