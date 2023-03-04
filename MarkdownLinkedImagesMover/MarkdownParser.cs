using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarkdownLinkedImagesMover;

internal static class MarkdownParser
{
    public static IEnumerable<string> ParseLinkedImages(string fileContent)
    {
        if (string.IsNullOrEmpty(fileContent))
        {
            return Array.Empty<string>();
        }

        var regex = new Regex(@"!\[\[(.+)\]\]");
        var matches = regex.Matches(fileContent);
        return matches.Select(match => match.Groups[1].Value).Distinct();
    }
}
