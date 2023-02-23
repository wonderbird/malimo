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

        Program.Main(new FileInfo("data/source/Testfile.md"));

        Assert.Equal("File 'Testfile.md' is not empty.\n", writer.ToString());
    }
}
