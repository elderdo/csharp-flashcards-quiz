namespace QuizletApp.Services;

public sealed class ConsoleUserInteraction : IUserInteraction
{
    public void WriteLine(string text = "")
    {
        Console.WriteLine(text);
    }

    public void Write(string text)
    {
        Console.Write(text);
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }
}
