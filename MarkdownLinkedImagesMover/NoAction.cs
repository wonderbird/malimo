using System.IO;

namespace MarkdownLinkedImagesMover;

internal class NoAction : IFileMover
{
    public void Move(FileInfo sourceFile, DirectoryInfo targetDir)
    {
    }
}