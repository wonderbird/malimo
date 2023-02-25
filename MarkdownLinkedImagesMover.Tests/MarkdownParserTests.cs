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
    public void StringWithMultipleLinks()
    {
        var expectedFiles = new FileInfo[]
        {
            new("link1.png"),
            new("link2.png"),
            new("link3.png")
        };

        var fileContent = CreateMarkdownWithLinksTo(expectedFiles);

        MarkdownParser.ParseLinkedImages(fileContent)
            .Should().BeEquivalentTo(expectedFiles, options => options.Using(new CompareFileInfo()));
    }

    [Fact]
    public void StringWithDuplicatedLinks()
    {
        var containedFiles = new FileInfo[]
        {
            new("A.png"),
            new("A.png")
        };
        var expectedFiles = new FileInfo[]
        {
            new("A.png")
        };

        var fileContent = CreateMarkdownWithLinksTo(containedFiles);

        MarkdownParser.ParseLinkedImages(fileContent)
            .Should().BeEquivalentTo(expectedFiles, options => options.Using(new CompareFileInfo()));
    }

    private static string CreateMarkdownWithLinksTo(IEnumerable<FileInfo> imageFiles)
    {
        var fileContentBuilder = new StringBuilder();

        fileContentBuilder.Append("# List of Images\n\n");

        imageFiles.ToList().ForEach(fileInfo =>
            fileContentBuilder.Append(CultureInfo.InvariantCulture, $"## {fileInfo.Name}:\n\n![[{fileInfo.Name}]]\n\n"));

        return fileContentBuilder.ToString();
    }
}
