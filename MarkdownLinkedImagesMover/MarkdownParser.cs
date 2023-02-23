using System.Collections.Generic;
using System.IO;

namespace MarkdownLinkedImagesMover;

internal static class MarkdownParser
{
    public static IEnumerable<FileInfo> ParseLinkedImages(string fileContent)
    {
        return new[] { new FileInfo("anything") };
    }
}