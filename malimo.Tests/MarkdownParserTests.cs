using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Text;

namespace malimo.Tests;

public class MarkdownParserTests
{
    private readonly MockFileSystem _fileSystemMock;
    private readonly FileInfo _file;

    public MarkdownParserTests()
    {
        _fileSystemMock = new MockFileSystem();
        _file = new FileInfo("/MarkdownFile.md");
    }

    [Fact]
    public void EmptyFile()
    {
        GivenEmptyFile();

        new MarkdownParser(_fileSystemMock).ParseLinkedImages(_file).Should().BeEmpty();
    }

    [Theory]
    [InlineData("No image links")]
    [InlineData("Invalid image link: ![Invalid Link]")]
    public void NonEmptyFile_NoLinkedImages(string fileContent)
    {
        GivenFileWithContent(fileContent);
        new MarkdownParser(_fileSystemMock).ParseLinkedImages(_file).Should().BeEmpty();
    }

    [Fact]
    public void FileWithMultipleLinks()
    {
        var expectedFiles = new[] { "link1.png", "link2.png", "link3.png" };

        GivenFileWithLinksTo(expectedFiles, "\n");

        new MarkdownParser(_fileSystemMock).ParseLinkedImages(_file).Should().BeEquivalentTo(expectedFiles);
    }

    [Fact]
    public void StringWithDuplicatedLinks()
    {
        var containedFiles = new[] { "A.png", "A.png" };
        var expectedFiles = new[] { "A.png" };

        GivenFileWithLinksTo(containedFiles, "\n");

        new MarkdownParser(_fileSystemMock).ParseLinkedImages(_file).Should().BeEquivalentTo(expectedFiles);
    }

    [Fact]
    public void StringWithMultipleLinksInSameLine()
    {
        var expectedFiles = new[] { "link1.png", "link2.png", "link3.png" };

        GivenFileWithLinksTo(expectedFiles, " ");

        new MarkdownParser(_fileSystemMock).ParseLinkedImages(_file).Should().BeEquivalentTo(expectedFiles);
    }

    private void GivenEmptyFile()
    {
        _fileSystemMock.AddEmptyFile(_file.FullName);
    }

    private void GivenFileWithContent(string fileContent)
    {
        _fileSystemMock.AddFile(_file.FullName, new MockFileData(fileContent));
    }

    private void GivenFileWithLinksTo(IEnumerable<string> expectedFiles, string separatedBy)
    {
        var fileContent = CreateMarkdownWithLinksTo(expectedFiles, separatedBy);

        _fileSystemMock.AddFile(_file.FullName, new MockFileData(fileContent));
    }

    private static string CreateMarkdownWithLinksTo(IEnumerable<string> imageFiles, string separatedBy)
    {
        var imageFilesAsLinks = imageFiles.Select(imageFile => $"![[{imageFile}]]");
        return string.Join(separatedBy, imageFilesAsLinks);
    }
}
