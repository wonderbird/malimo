using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text.RegularExpressions;

namespace malimo;

internal class MarkdownParser
{
    private readonly IFileSystem _fileSystem;

    public MarkdownParser(IFileSystem fileSystem) => _fileSystem = fileSystem;

    public List<string> ParseLinkedImages(FileSystemInfo markdownFile)
    {
        var fileContent = _fileSystem.File.ReadAllText(markdownFile.FullName);
        return ParseLinkedImages(fileContent).ToList();
    }

    private static IEnumerable<string> ParseLinkedImages(string fileContent)
    {
        if (string.IsNullOrEmpty(fileContent))
        {
            return Array.Empty<string>();
        }

        var regex = new Regex(@"!\[\[(.+?)\]\]");
        var matches = regex.Matches(fileContent);
        return matches.Select(match => match.Groups[1].Value).Distinct();
    }
}
