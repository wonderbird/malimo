using System;
using System.Globalization;
using System.IO;

namespace MarkdownLinkedImagesMover.Tests;

public sealed class TestDirectory : IDisposable
{
    private DirectoryInfo _baseDir;

    public DirectoryInfo SourceDir { get; private set; }

    public DirectoryInfo TargetDir { get; private set; }

    public void Dispose() => Delete();

    public static TestDirectory Create()
    {
        var result = new TestDirectory();
        result.CreateDirectories();
        result.CopySeedToSource();
        return result;
    }

    private void CreateDirectories()
    {
        var dateString = DateTime.Now.ToString("yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
        var uid = Guid.NewGuid();
        _baseDir = new DirectoryInfo($"data/test-{dateString}-{uid}");
        _baseDir.Create();

        SourceDir = new DirectoryInfo(Path.Combine(_baseDir.FullName, "source"));
        SourceDir.Create();

        TargetDir = new DirectoryInfo(Path.Combine(_baseDir.FullName, "target"));
        TargetDir.Create();
    }

    private void CopySeedToSource()
    {
        var seedDir = new DirectoryInfo("data/seed");
        foreach (var seedFile in seedDir.GetFiles())
        {
            var targetFile = Path.Combine(SourceDir.FullName, seedFile.Name);
            seedFile.CopyTo(targetFile);
        }
    }

    private void Delete() => _baseDir?.Delete(recursive: true);

    private TestDirectory() { }
}
