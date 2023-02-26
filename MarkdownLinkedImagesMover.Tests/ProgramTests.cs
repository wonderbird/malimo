using System;
using System.IO;

namespace MarkdownLinkedImagesMover.Tests;

public class ProgramTests
{
    [Fact]
    public void ProcessTestfile()
    {
        var writer = new StringWriter();
        Console.SetOut(writer);

        Program.Main(new FileInfo("data/source/Testfile.md"), new DirectoryInfo("data/target"));

        var output = writer.ToString();

        Assert.Matches("Target folder: '.*/data/target'", output);

        Assert.Contains("File 'Testfile.md' contains", output);
        Assert.Contains("'noun-starship-3799189.png'", output);
        Assert.Contains("'noun-island-1479438.png'", output);
    }
}
