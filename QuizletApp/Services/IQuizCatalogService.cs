using QuizletApp.Models;

namespace QuizletApp.Services;

public interface IQuizCatalogService
{
    string GetDataDirectory();
    IReadOnlyList<string> GetSeparators();
    IReadOnlyList<string> GetQuizFiles(string dataDirectory);
    List<Flashcard> LoadCards(string quizFile, IReadOnlyList<string> separators);
}
