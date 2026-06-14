using QuizletApp.Models;

namespace QuizletApp.Services;

public interface IQuizSettingsProvider
{
    QuizSettings LoadSettings();
}
