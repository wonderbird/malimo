using System.IO;

namespace MarkdownLinkedImagesMover;

internal static class FileMover
{
    public static void Move(FileInfo sourceFile, DirectoryInfo targetDir)
    {
        var destinationFile = Path.Combine(targetDir.FullName, sourceFile.Name);
        sourceFile.MoveTo(destinationFile);
    }
}