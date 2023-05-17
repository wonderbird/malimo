using malimo.TestDirectoryHelper;
using TestProcessWrapper;

namespace malimo.Integration.Tests;

public class ProgramTests
{
    [Fact]
    public void VerboseDryRun()
    {
        using var testDir = TestDirectory.Create();

#if DEBUG
        const BuildConfiguration buildConfiguration = BuildConfiguration.Debug;
#else
        const BuildConfiguration buildConfiguration = BuildConfiguration.Release;
#endif
        var process = new TestProcessWrapper.TestProcessWrapper("malimo", false, buildConfiguration);
        var sourceFile = new FileInfo(Path.Combine(testDir.SourceDir.FullName, "Testfile.md"));
        var targetDir = testDir.TargetDir;

        process.AddCommandLineArgument("--file", sourceFile.FullName);
        process.AddCommandLineArgument("--target-dir", targetDir.FullName);
        process.AddCommandLineArgument("--dry-run");

        process.Start();
        process.WaitForProcessExit();
        var hasProcessExitedGracefully = process.HasExited;

        process.ForceTermination();

        Assert.Matches("Would move '.*/noun-island-1479438.png'", process.RecordedOutput);
        Assert.True(hasProcessExitedGracefully, "malimo should exit gracefully");
    }
}