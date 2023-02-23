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
}