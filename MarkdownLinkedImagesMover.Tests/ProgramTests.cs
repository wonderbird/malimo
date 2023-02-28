using System;
using System.IO;

namespace MarkdownLinkedImagesMover.Tests;

public sealed class ProgramTests : IDisposable
{
    private readonly TestDirectory _testDir;

    public ProgramTests() => _testDir = TestDirectory.Create();

    [Fact]
    public void ProcessTestfile()
    {
        var writer = new StringWriter();
        Console.SetOut(writer);

        Program.Main(new FileInfo(Path.Combine(_testDir.SourceDir.FullName, "Testfile.md")), _testDir.TargetDir);

        var output = writer.ToString();

        Assert.Contains($"Target folder: '{_testDir.TargetDir.FullName}'", output);
        Assert.Contains("File 'Testfile.md' contains", output);
        Assert.Contains("'noun-starship-3799189.png'", output);
        Assert.Contains("'noun-island-1479438.png'", output);
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

    ~ProgramTests()
    {
        ReleaseUnmanagedResources();
    }
}
