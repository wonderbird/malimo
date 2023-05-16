using System.Globalization;

namespace malimo.TestDirectoryHelper;

public sealed class TestDirectory : IDisposable
{
    private readonly DirectoryInfo _baseDir;

    public DirectoryInfo SourceDir { get; }

    public DirectoryInfo TargetDir { get; }

    public void Dispose() => Delete();

    private TestDirectory()
    {
        var dateString = DateTime.Now.ToString("yyyyMMddTHHmmss", CultureInfo.InvariantCulture);
        var uid = Guid.NewGuid();

        _baseDir = new DirectoryInfo($"data/test-{dateString}-{uid}");
        SourceDir = new DirectoryInfo(Path.Combine(_baseDir.FullName, "source"));
        TargetDir = new DirectoryInfo(Path.Combine(_baseDir.FullName, "target"));
    }

    public static TestDirectory Create()
    {
        var result = new TestDirectory();
        result.CreateDirectories();
        result.CopySeedToSource();
        return result;
    }

    private void CreateDirectories()
    {
        _baseDir.Create();
        SourceDir.Create();
        TargetDir.Create();
    }

    private void CopySeedToSource()
    {
        CopyDirectoryRecursively(new DirectoryInfo("data/seed"), SourceDir);
    }

    private void CopyDirectoryRecursively(DirectoryInfo sourceDirectory, DirectoryInfo targetDirectory)
    {
        if (!targetDirectory.Exists)
        {
            targetDirectory.Create();
        }

        foreach (var file in sourceDirectory.GetFiles())
        {
            var targetFilePath = Path.Combine(targetDirectory.FullName, file.Name);
            file.CopyTo(targetFilePath);
        }

        foreach (var subdirectory in sourceDirectory.GetDirectories())
        {
            var targetSubdirectory = new DirectoryInfo(Path.Combine(SourceDir.FullName, subdirectory.Name));
            CopyDirectoryRecursively(subdirectory, targetSubdirectory);
        }
    }

    private void Delete() => _baseDir.Delete(recursive: true);
}
