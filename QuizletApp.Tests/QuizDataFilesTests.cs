using QuizletApp.Services;

namespace QuizletApp.Tests;

public class QuizDataFilesTests
{
    [Fact]
    public void EveryQuizLineParsesWithConfiguredSeparators()
    {
        var repoRoot = FindRepoRoot();
        var settingsPath = Path.Combine(repoRoot, "QuizletApp", "quizsettings.json");
        var dataDirectory = Path.Combine(repoRoot, "QuizletApp", "data");

        var settingsProvider = new QuizSettingsProvider(["--settings", settingsPath]);
        var separatorProvider = new SeparatorProvider([]);
        var parser = new FlashcardParser();

        var separators = separatorProvider.BuildSeparators(settingsProvider.LoadSettings());
        var quizFiles = Directory.EnumerateFiles(dataDirectory, "*.txt", SearchOption.TopDirectoryOnly).ToList();

        Assert.NotEmpty(quizFiles);

        foreach (var file in quizFiles)
        {
            var lines = File.ReadAllLines(file);
            Assert.NotEmpty(lines);

            foreach (var rawLine in lines)
            {
                var line = rawLine.Trim();
                if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
                {
                    continue;
                }

                var parsed = parser.TryParse(line, separators, out var card);
                Assert.True(parsed, $"Could not parse line in {Path.GetFileName(file)}: {line}");
                Assert.False(string.IsNullOrWhiteSpace(card.Question));
                Assert.False(string.IsNullOrWhiteSpace(card.Answer));
            }
        }
    }

    private static string FindRepoRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (Directory.Exists(Path.Combine(dir.FullName, "QuizletApp"))
                && Directory.Exists(Path.Combine(dir.FullName, "QuizletApp.Tests")))
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException("Could not locate repository root from test output directory.");
    }
}
