namespace QuizletApp.Services;

public interface IUserInteraction
{
    void WriteLine(string text = "");
    void Write(string text);
    string? ReadLine();
}
