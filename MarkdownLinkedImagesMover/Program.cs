using System;
using System.IO;

namespace MarkdownLinkedImagesMover;

public static class Program
{
    /// <summary>
    /// Move all images referred to by the given markdown file to a folder.
    /// </summary>
    /// <param name="file">Markdown file containing the references to image files which shall be moved</param>
    public static void Main(FileInfo file)
    {
        Console.WriteLine($"Processed 0 images from '{file.Name}'.");
    }
}
