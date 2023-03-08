using System.IO;

namespace MarkdownLinkedImagesMover;

internal interface IFileMover
{
    void Move(FileInfo sourceFile, DirectoryInfo targetDir);
}
