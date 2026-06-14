using QuizletApp.Models;

namespace QuizletApp.Services;

public interface IFlashcardParser
{
    bool TryParse(string line, IReadOnlyList<string> separators, out Flashcard card);
}
