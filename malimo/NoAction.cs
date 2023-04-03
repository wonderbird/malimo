using System.IO;

namespace malimo;

internal class NoAction : IFileMover
{
    public void Move(FileInfo sourceFile, DirectoryInfo targetDir) { }
}
