using QuizletApp.Models;

namespace QuizletApp.Services;

public interface IFlashcardLoader
{
    List<Flashcard> Load(string path, IReadOnlyList<string> separators);
}
