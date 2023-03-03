using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Moq;

namespace MarkdownLinkedImagesMover.Tests;

public sealed class AppTests : IDisposable
{
    private readonly TestDirectory _testDir;

    public AppTests() => _testDir = TestDirectory.Create();

    // TODO: Fix static code analysis warnings
#pragma warning disable CA1822
#pragma warning disable CA1848
#pragma warning disable CA2254
    [Fact]
    public void ProcessTestfile()
    {
        var loggerMock = new Mock<ILogger<App>>();
        var sourceFile = new FileInfo(Path.Combine(_testDir.SourceDir.FullName, "Testfile.md"));

        new App(loggerMock.Object).Run(sourceFile, _testDir.TargetDir);

        loggerMock.VerifyLog(logger => logger.LogInformation("Target folder: '{@TargetFolder}'", _testDir.TargetDir.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("File '{@SourceFile}' contains", sourceFile.FullName));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-starship-3799189.png"));
        loggerMock.VerifyLog(logger => logger.LogInformation("- '{@ImageFile}'", "noun-island-1479438.png"));
    }

    private void ReleaseUnmanagedResources()
    {
        _testDir.Delete();
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    ~AppTests()
    {
        ReleaseUnmanagedResources();
    }
#pragma warning disable CA2254
#pragma warning restore CA1848
#pragma warning restore CA1822
}
