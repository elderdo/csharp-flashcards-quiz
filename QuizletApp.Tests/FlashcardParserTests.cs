using QuizletApp.Services;

namespace QuizletApp.Tests;

public class FlashcardParserTests
{
    [Theory]
    [InlineData("Question\tAnswer", "\t")]
    [InlineData("Question###Answer", "###")]
    [InlineData("Question~Answer", "~")]
    public void TryParse_ParsesValidCard(string line, string separator)
    {
        var parser = new FlashcardParser();

        var ok = parser.TryParse(line, [separator], out var card);

        Assert.True(ok);
        Assert.Equal("Question", card.Question);
        Assert.Equal("Answer", card.Answer);
    }

    [Fact]
    public void TryParse_ReturnsFalseForInvalidLine()
    {
        var parser = new FlashcardParser();

        var ok = parser.TryParse("NoSeparatorHere", ["\t", "###"], out _);

        Assert.False(ok);
    }
}
