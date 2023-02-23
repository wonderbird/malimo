using System;
using System.IO;
using Xunit;

namespace MarkdownLinkedImagesMover.Tests;

public class ProgramTests
{
    [Fact]
    public void ProcessTestfile()
    {
        var writer = new StringWriter();
        Console.SetOut(writer);

        Program.Main(new FileInfo("data/source/Testfile.md"));

        Assert.Equal("Processed 1 image(s) from 'Testfile.md'.\n", writer.ToString());
    }
}
