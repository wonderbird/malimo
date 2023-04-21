using System.IO;
using Microsoft.Extensions.Logging;
using Moq;

namespace malimo.Tests;

public class NoActionTests
{
    [Fact]
    public void GivenTwoExistingImagesAndVerboseLogging_ThenLogsDetailsOnMoveOperation()
    {
        var sourceFile = new FileInfo("/noun-island-1479438.png");
        var targetDir = new DirectoryInfo("/MockedTargetDir");

        var loggerMock = new Mock<ILogger<NoAction>>();

        new NoAction(loggerMock.Object).Move(sourceFile, targetDir);

        loggerMock.VerifyLog(logger => logger.LogDebug("Would move '{@ImageFile}' to '{@TargetFolder}'", sourceFile.FullName,
            targetDir.FullName));
    }
}