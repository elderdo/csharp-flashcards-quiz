using QuizletApp.Services;

namespace QuizletApp.Tests;

public class FlashcardLoaderTests
{
    [Fact]
    public void Load_IgnoresCommentsAndInvalidLines()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllLines(tempFile,
            [
                "# comment",
                "",
                "Question 1\tAnswer 1",
                "invalid line",
                "Question 2###Answer 2"
            ]);

            var loader = new FlashcardLoader(new FlashcardParser());

            var cards = loader.Load(tempFile, ["###", "\t"]);

            Assert.Equal(2, cards.Count);
            Assert.Equal("Question 1", cards[0].Question);
            Assert.Equal("Answer 2", cards[1].Answer);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
