using System.IO;

namespace malimo;

internal interface IFileMover
{
    void Move(FileInfo sourceFile, DirectoryInfo targetDir);
}
