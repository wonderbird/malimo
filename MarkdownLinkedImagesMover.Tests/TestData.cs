using System.IO;

namespace MarkdownLinkedImagesMover.Tests;

public static class TestData
{
    // TODO: Allow parallel testing. The current structure of single (source, target) folders forces tests to be run sequentially.
    public static void Setup()
    {
        var sourceDir = new DirectoryInfo("data/source");
        if (sourceDir.Exists)
        {
            sourceDir.Delete(recursive: true);
        }

        sourceDir.Create();

        var targetDir = new DirectoryInfo("data/target");
        if (targetDir.Exists)
        {
            targetDir.Delete(recursive: true);
        }

        targetDir.Create();

        var seedDir = new DirectoryInfo("data/seed");
        foreach (var seedFile in seedDir.GetFiles())
        {
            var targetFile = Path.Combine(sourceDir.FullName, seedFile.Name);
            seedFile.CopyTo(targetFile);
        }
    }
}
