using System.IO;

namespace MarkdownLinkedImagesMover.Tests.IntegrativeTests;

public sealed class ProgramTests
{
    [Fact]
    public void MarkdownFileWithTwoImages()
    {
        using var testDir = TestDirectory.Create();

        var sourceFile = new FileInfo(Path.Combine(testDir.SourceDir.FullName, "Testfile.md"));

        Program.Main(sourceFile, testDir.TargetDir);

        AssertFileExists("noun-island-1479438.png", testDir.TargetDir);
        AssertFileExists("noun-starship-3799189.png", testDir.TargetDir);
    }

    private static void AssertFileExists(string fileName, FileSystemInfo dir)
    {
        var targetFile = new FileInfo(Path.Combine(dir.FullName, fileName));
        Assert.True(targetFile.Exists, $"file '{targetFile.FullName}' should exist");
    }
}
