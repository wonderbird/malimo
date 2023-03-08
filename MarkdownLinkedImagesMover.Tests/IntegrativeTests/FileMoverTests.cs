using System.IO;

namespace MarkdownLinkedImagesMover.Tests.IntegrativeTests;

public sealed class FileMoverTests
{
    [Fact]
    public void MoveExistingFileToValidDestination()
    {
        using var testDir = TestDirectory.Create();

        var sourceFile = new FileInfo(Path.Combine(testDir.SourceDir.FullName, "Testfile.md"));
        var targetDir = testDir.TargetDir;

        new FileMover().Move(sourceFile, targetDir);

        var hasMoved = new FileInfo(Path.Combine(targetDir.FullName, sourceFile.Name)).Exists;
        Assert.True(hasMoved, "file should be moved to target folder");
    }
}
