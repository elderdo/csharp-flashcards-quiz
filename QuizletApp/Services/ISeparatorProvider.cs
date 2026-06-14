using QuizletApp.Models;

namespace QuizletApp.Services;

public interface ISeparatorProvider
{
    IReadOnlyList<string> BuildSeparators(QuizSettings settings);
}
