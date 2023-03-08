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

        var fileContent = CreateMarkdownWithLinksTo(expectedFiles, "\n");

        MarkdownParser.ParseLinkedImages(fileContent).Should().BeEquivalentTo(expectedFiles);
    }

    [Fact]
    public void StringWithDuplicatedLinks()
    {
        var containedFiles = new[] { "A.png", "A.png" };
        var expectedFiles = new[] { "A.png" };

        var fileContent = CreateMarkdownWithLinksTo(containedFiles, "\n");

        MarkdownParser.ParseLinkedImages(fileContent).Should().BeEquivalentTo(expectedFiles);
    }

    [Fact]
    public void StringWithMultipleLinksInSameLine()
    {
        var expectedFiles = new[] { "link1.png", "link2.png", "link3.png" };

        var fileContent = CreateMarkdownWithLinksTo(expectedFiles, " ");

        MarkdownParser.ParseLinkedImages(fileContent).Should().BeEquivalentTo(expectedFiles);
    }

    private static string CreateMarkdownWithLinksTo(IEnumerable<string> imageFiles, string separatedBy)
    {
        var imageFilesAsLinks = imageFiles.Select(imageFile => $"![[{imageFile}]]");
        return string.Join(separatedBy, imageFilesAsLinks);
    }
}
