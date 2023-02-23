using System;
using System.Collections.Generic;
using System.IO;

namespace MarkdownLinkedImagesMover;

internal static class MarkdownParser
{
    public static IEnumerable<FileInfo> ParseLinkedImages(string fileContent)
    {
        if (string.IsNullOrEmpty(fileContent))
        {
            return Array.Empty<FileInfo>();
        }

        return new[] { new FileInfo("anything") };
    }
}