using QuizletApp.Models;

namespace QuizletApp.Services;

public interface IQuizSessionService
{
    QuizSession StartSession(IReadOnlyList<Flashcard> cards, bool shuffle = true);
    bool TryGetPrompt(QuizSession session, out QuizPrompt prompt);
    string RevealAnswer(QuizSession session);
    bool MoveNext(QuizSession session);
}
