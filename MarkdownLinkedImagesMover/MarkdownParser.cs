using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        var matches = regex.Matches(fileContent);
        return matches.Select(match => new FileInfo(match.Groups[1].Value)).Distinct(new CompareFileInfo());
    }
}