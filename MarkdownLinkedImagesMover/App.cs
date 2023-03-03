using System;
using System.IO;
using System.Linq;

namespace MarkdownLinkedImagesMover;

internal class App
{
    // For now suppress the warning about Run may be a static method, because
    // TODO: Replace Console.WriteLine by Logging injected via Dependency Injection
#pragma warning disable CA1822
    public void Run(FileInfo file, DirectoryInfo targetDir)
    {
        var fileContent = File.ReadAllText(file.FullName);

        Console.WriteLine($"Target folder: '{targetDir.FullName}'");
        Console.WriteLine($"File '{file.Name}' contains");

        var images = MarkdownParser.ParseLinkedImages(fileContent);
        var imageNames = string.Join("", images.Select(f => $"- '{f.Name}'\n"));
        Console.WriteLine(imageNames);
    }
#pragma warning restore CA1822
}