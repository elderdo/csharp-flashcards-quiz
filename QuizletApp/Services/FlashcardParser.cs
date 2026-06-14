using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class FlashcardParser : IFlashcardParser
{
    public bool TryParse(string line, IReadOnlyList<string> separators, out Flashcard card)
    {
        foreach (var separator in separators)
        {
            var splitIndex = line.IndexOf(separator, StringComparison.Ordinal);
            if (splitIndex <= 0)
            {
                continue;
            }

            var question = line[..splitIndex].Trim();
            var answerStart = splitIndex + separator.Length;
            var answer = line[answerStart..].Trim();

            if (question.Length == 0 || answer.Length == 0)
            {
                continue;
            }

            card = new Flashcard(question, answer);
            return true;
        }

        card = default;
        return false;
    }
}
