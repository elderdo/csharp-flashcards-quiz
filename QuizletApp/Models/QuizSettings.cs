namespace QuizletApp.Models;

public sealed class QuizSettings
{
    public string? DataDirectory { get; init; }
    public List<string>? CustomSeparators { get; init; }
}
