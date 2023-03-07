using System.Collections.Generic;
using System.Globalization;
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
        var expectedFiles = new[] { "link1.png", "link2.png", "link3.png" };

        var fileContent = CreateMarkdownWithLinksTo(expectedFiles);

        MarkdownParser.ParseLinkedImages(fileContent).Should().BeEquivalentTo(expectedFiles);
    }

    [Fact]
    public void StringWithDuplicatedLinks()
    {
        var containedFiles = new[] { "A.png", "A.png" };
        var expectedFiles = new[] { "A.png" };

        var fileContent = CreateMarkdownWithLinksTo(containedFiles);

        MarkdownParser.ParseLinkedImages(fileContent).Should().BeEquivalentTo(expectedFiles);
    }

    private static string CreateMarkdownWithLinksTo(IEnumerable<string> imageFiles)
    {
        var fileContentBuilder = new StringBuilder();

        fileContentBuilder.Append("# List of Images\n\n");

        imageFiles
            .ToList()
            .ForEach(
                fileName =>
                    fileContentBuilder.Append(CultureInfo.InvariantCulture, $"## {fileName}:\n\n![[{fileName}]]\n\n")
            );

        return fileContentBuilder.ToString();
    }
}
