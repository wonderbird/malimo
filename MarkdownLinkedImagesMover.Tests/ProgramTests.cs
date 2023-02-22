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

        Program.Main(null);

        Assert.Equal("Processed 0 images.\n", writer.ToString());
    }
}
