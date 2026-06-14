using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class QuizCatalogService(
    IQuizSettingsProvider settingsProvider,
    ISeparatorProvider separatorProvider,
    IFlashcardLoader flashcardLoader) : IQuizCatalogService
{
    public string GetDataDirectory()
    {
        var settings = settingsProvider.LoadSettings();
        return ResolveDataDirectory(settings.DataDirectory);
    }

    public IReadOnlyList<string> GetSeparators()
    {
        var settings = settingsProvider.LoadSettings();
        return separatorProvider.BuildSeparators(settings);
    }

    public IReadOnlyList<string> GetQuizFiles(string dataDirectory)
    {
        return Directory
            .EnumerateFiles(dataDirectory)
            .Where(IsQuizFile)
            .OrderBy(path => path, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public List<Flashcard> LoadCards(string quizFile, IReadOnlyList<string> separators)
    {
        return flashcardLoader.Load(quizFile, separators);
    }

    private static string ResolveDataDirectory(string? configuredPath)
    {
        if (string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "data");
        }

        return Path.IsPathRooted(configuredPath)
            ? configuredPath
            : Path.Combine(Directory.GetCurrentDirectory(), configuredPath);
    }

    private static bool IsQuizFile(string path)
    {
        var extension = Path.GetExtension(path);
        return extension.Equals(".txt", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".tsv", StringComparison.OrdinalIgnoreCase)
            || extension.Equals(".csv", StringComparison.OrdinalIgnoreCase);
    }
}
