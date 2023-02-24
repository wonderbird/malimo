using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MarkdownLinkedImagesMover;

internal static class MarkdownParser
{
    public static IEnumerable<FileInfo> ParseLinkedImages(string fileContent)
    {
        if (string.IsNullOrEmpty(fileContent))
        {
            return Array.Empty<FileInfo>();
        }

        var regex = new Regex(@"!\[\[(.+)\]\]");
        var match = regex.Match(fileContent);

        if (!match.Success)
        {
            return Array.Empty<FileInfo>();
        }

        return new[] { new FileInfo(match.Groups[1].Value) };

    }
}