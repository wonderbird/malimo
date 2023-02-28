using System.IO;

namespace MarkdownLinkedImagesMover.Tests;

public class FileMoverTests
{
    [Fact]
    public void MoveExistingFileToValidDestination()
    {
        var sourceFile = new FileInfo("data/source/Testfile.md");
        var targetDir = new DirectoryInfo("data/target");

        FileMover.Move(sourceFile, targetDir);

        var hasMoved = new FileInfo(Path.Combine(targetDir.FullName, sourceFile.Name)).Exists;
        Assert.True(hasMoved, "file should be moved to target folder");
    }
}