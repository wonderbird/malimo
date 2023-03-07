using System;
using System.IO;

namespace MarkdownLinkedImagesMover.Tests;

public sealed class ProgramTests : IDisposable
{
    private readonly TestDirectory _testDir;

    public ProgramTests() => _testDir = TestDirectory.Create();

    public void Dispose() => _testDir.Delete();

    [Fact]
    public void MarkdownFileWithTwoImages()
    {
        var sourceFile = new FileInfo(Path.Combine(_testDir.SourceDir.FullName, "Testfile.md"));

        Program.Main(sourceFile, _testDir.TargetDir);

        AssertFileExists("noun-island-1479438.png", _testDir.TargetDir);
        AssertFileExists("noun-starship-3799189.png", _testDir.TargetDir);
    }

    private static void AssertFileExists(string fileName, FileSystemInfo dir)
    {
        var targetFile = new FileInfo(Path.Combine(dir.FullName, fileName));
        Assert.True(targetFile.Exists, $"file '{targetFile.FullName}' should exist");
    }
}