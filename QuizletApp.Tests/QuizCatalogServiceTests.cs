using QuizletApp.Models;
using QuizletApp.Services;

namespace QuizletApp.Tests;

public class QuizCatalogServiceTests
{
    [Fact]
    public void GetQuizFiles_ReturnsOnlySupportedExtensions()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"quizcatalog-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);

        try
        {
            File.WriteAllText(Path.Combine(tempDir, "a.txt"), "q\ta");
            File.WriteAllText(Path.Combine(tempDir, "b.tsv"), "q\ta");
            File.WriteAllText(Path.Combine(tempDir, "c.csv"), "q,a");
            File.WriteAllText(Path.Combine(tempDir, "ignore.md"), "x");

            var settingsProvider = new StubSettingsProvider(new QuizSettings { DataDirectory = tempDir });
            var separatorProvider = new SeparatorProvider([]);
            var loader = new FlashcardLoader(new FlashcardParser());
            var service = new QuizCatalogService(settingsProvider, separatorProvider, loader);

            var files = service.GetQuizFiles(tempDir);

            Assert.Equal(3, files.Count);
            Assert.DoesNotContain(files, f => f.EndsWith(".md", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Fact]
    public void GetDataDirectory_ResolvesRelativeToCurrentDirectory()
    {
        var settingsProvider = new StubSettingsProvider(new QuizSettings { DataDirectory = "data" });
        var service = new QuizCatalogService(settingsProvider, new SeparatorProvider([]), new FlashcardLoader(new FlashcardParser()));

        var resolved = service.GetDataDirectory();

        Assert.Equal(Path.Combine(Directory.GetCurrentDirectory(), "data"), resolved);
    }

    private sealed class StubSettingsProvider(QuizSettings settings) : IQuizSettingsProvider
    {
        public QuizSettings LoadSettings() => settings;
    }
}
