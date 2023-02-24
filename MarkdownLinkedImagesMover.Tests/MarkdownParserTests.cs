using System.IO;

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
}
