using System.IO;
using malimo.TestDirectoryHelper;

namespace malimo.Tests.IntegrativeTests;

public sealed class ProgramTests
{
    [Fact]
    public void WithSourceDirAndWithoutDryRunParameters()
    {
        using var testDir = TestDirectory.Create();

        var sourceFile = new FileInfo(Path.Combine(testDir.SourceDir.FullName, "ImagesInSubdirectory.md"));

        Program.Main(
            sourceFile,
            testDir.TargetDir,
            new DirectoryInfo(Path.Combine(testDir.SourceDir.FullName, "subdirectory"))
        );

        var sourceDir = new DirectoryInfo(Path.Combine(testDir.SourceDir.FullName, "subdirectory"));
        AssertFileDoesNotExist("noun-plant-5382590.png", sourceDir);
        AssertFileExists("noun-plant-5382590.png", testDir.TargetDir);
    }

    private static void AssertFileDoesNotExist(string fileName, FileSystemInfo dir) =>
        AssertFileStatus(FileStatus.DoesNotExist, fileName, dir);

    private static void AssertFileExists(string fileName, FileSystemInfo dir) =>
        AssertFileStatus(FileStatus.DoesExist, fileName, dir);

    private static void AssertFileStatus(FileStatus expected, string fileName, FileSystemInfo dir)
    {
        var file = new FileInfo(Path.Combine(dir.FullName, fileName));

        if (expected == FileStatus.DoesExist)
        {
            Assert.True(file.Exists, $"file '{file.FullName}' should exist");
        }
        else
        {
            Assert.False(file.Exists, $"file '{file.FullName}' should not exist");
        }
    }

    private enum FileStatus
    {
        DoesNotExist = 0,
        DoesExist
    }
}
