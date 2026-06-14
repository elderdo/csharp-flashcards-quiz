using QuizletApp.Models;
using QuizletApp.Services;

namespace QuizletApp.Tests;

public class QuizPipelineIntegrationTests
{
    [Fact]
    public void Pipeline_LoadsCardsUsingCombinedSeparators()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"quizletapp-tests-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);

        try
        {
            var settingsPath = Path.Combine(tempDir, "quizsettings.json");
            var quizPath = Path.Combine(tempDir, "sample.txt");

            File.WriteAllText(settingsPath, "{\"CustomSeparators\":[\"###\"]}");
            File.WriteAllLines(quizPath,
            [
                "Term 1###Definition 1",
                "Term 2\tDefinition 2"
            ]);

            var settingsProvider = new QuizSettingsProvider(["--settings", settingsPath]);
            var separatorProvider = new SeparatorProvider(["--separator", "@@@"]);
            var loader = new FlashcardLoader(new FlashcardParser());

            var settings = settingsProvider.LoadSettings();
            var separators = separatorProvider.BuildSeparators(settings);
            var cards = loader.Load(quizPath, separators);

            Assert.Equal(2, cards.Count);
            Assert.Contains(cards, c => c == new Flashcard("Term 1", "Definition 1"));
            Assert.Contains(cards, c => c == new Flashcard("Term 2", "Definition 2"));
            Assert.Contains("###", separators);
            Assert.Contains("@@@", separators);
            Assert.Contains("\t", separators);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}
