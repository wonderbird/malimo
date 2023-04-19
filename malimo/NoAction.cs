using System.IO;
using Microsoft.Extensions.Logging;

namespace malimo;

internal class NoAction : IFileMover
{
    private readonly ILogger<NoAction> _logger;

    public NoAction(ILogger<NoAction> logger) => _logger = logger;

    public void Move(FileInfo sourceFile, DirectoryInfo targetDir)
    {
        // TODO: Add Test
        _logger.LogInformation("Would move '{@SourceFile}' to '{@TargetFolder}'", sourceFile.FullName, targetDir.FullName);
    }
}
