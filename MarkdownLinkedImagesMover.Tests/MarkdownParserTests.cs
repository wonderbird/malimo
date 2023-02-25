using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MarkdownLinkedImagesMover.Tests;

public class MarkdownParserTests
{
    [Fact]
    public void NullArgument()
    {
        MarkdownParser.ParseLinkedImages(null).Should().BeEmpty();
    }

    [Fact]
    public void EmptyString()
    {
        MarkdownParser.ParseLinkedImages("").Should().BeEmpty();
    }

    [Theory]
    [InlineData("No image links")]
    [InlineData("Invalid image link: ![Invalid Link]")]
    public void NonEmptyString_NoLinkedImages(string fileContent)
    {
        MarkdownParser.ParseLinkedImages(fileContent).Should().BeEmpty();
    }

    [Fact]
    public void StringWithSingleLink()
    {
        var expectedFiles = new[] { new FileInfo("first link.png") };
        var fileContent = $"![[{expectedFiles[0].Name}]]"; 

        MarkdownParser.ParseLinkedImages(fileContent)
            .Should().BeEquivalentTo(expectedFiles, options => options.Including(f => f.Name));
    }
    
    [Fact]
    public void StringWithMultipleLinks()
    {
        var expectedFiles = new List<FileInfo>
        {
            new("link1.png"),
            new("link2.png"),
            new("link3.png")
        };

        var fileContentBuilder = new StringBuilder();
        fileContentBuilder.Append("# List of Images\n\n");
        expectedFiles.ForEach(fileInfo => fileContentBuilder.Append(CultureInfo.InvariantCulture, $"## {fileInfo.Name}:\n\n![[{fileInfo.Name}]]\n\n"));

        MarkdownParser.ParseLinkedImages(fileContentBuilder.ToString())
            .Should().BeEquivalentTo(expectedFiles, options => options.Including(f => f.Name));
    }
}
