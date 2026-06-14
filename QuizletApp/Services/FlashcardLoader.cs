using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class FlashcardLoader(IFlashcardParser parser) : IFlashcardLoader
{
    public List<Flashcard> Load(string path, IReadOnlyList<string> separators)
    {
        var cards = new List<Flashcard>();

        foreach (var rawLine in File.ReadLines(path))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
            {
                continue;
            }

            if (parser.TryParse(line, separators, out var card))
            {
                cards.Add(card);
            }
        }

        return cards;
    }
}
