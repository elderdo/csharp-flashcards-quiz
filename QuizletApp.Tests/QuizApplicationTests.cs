using QuizletApp.Models;
using QuizletApp.Services;

namespace QuizletApp.Tests;

public class QuizApplicationTests
{
    [Fact]
    public async Task RunAsync_WhenNoQuizFiles_PrintsHelpfulMessage()
    {
        var catalog = new StubCatalogService
        {
            DataDirectory = "data",
            Separators = ["\t"],
            QuizFiles = []
        };
        var ui = new FakeUi([]);
        var app = new QuizApplication(catalog, new QuizSessionService(), ui);

        await app.RunAsync();

        Assert.Contains(ui.Output, line => line.Contains("No quiz files found", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task RunAsync_WithInvalidThenValidSelection_LoadsSelectedQuizAndAllowsQuit()
    {
        var expectedFile = Path.Combine("data", "b.txt");
        var catalog = new StubCatalogService
        {
            DataDirectory = "data",
            Separators = ["\t"],
            QuizFiles = [Path.Combine("data", "a.txt"), expectedFile],
            Cards = [new Flashcard("Q1", "A1")]
        };
        var ui = new FakeUi(["x", "2", "q"]);
        var app = new QuizApplication(catalog, new QuizSessionService(), ui);

        await app.RunAsync();

        Assert.Equal(expectedFile, catalog.LastLoadedQuizFile);
        Assert.Contains(ui.Output, line => line.Contains("Invalid selection", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(ui.Output, line => line.Contains("Q: Q1", StringComparison.Ordinal));
    }

    private sealed class StubCatalogService : IQuizCatalogService
    {
        public string DataDirectory { get; init; } = "data";
        public IReadOnlyList<string> Separators { get; init; } = ["\t"];
        public IReadOnlyList<string> QuizFiles { get; init; } = [];
        public List<Flashcard> Cards { get; init; } = [];
        public string? LastLoadedQuizFile { get; private set; }

        public string GetDataDirectory() => DataDirectory;
        public IReadOnlyList<string> GetSeparators() => Separators;
        public IReadOnlyList<string> GetQuizFiles(string dataDirectory) => QuizFiles;
        public List<Flashcard> LoadCards(string quizFile, IReadOnlyList<string> separators)
        {
            LastLoadedQuizFile = quizFile;
            return Cards;
        }
    }

    private sealed class FakeUi(IEnumerable<string> inputs) : IUserInteraction
    {
        private readonly Queue<string> _inputs = new(inputs);
        public List<string> Output { get; } = [];

        public void WriteLine(string text = "") => Output.Add(text);
        public void Write(string text) => Output.Add(text);

        public string? ReadLine()
        {
            return _inputs.Count == 0 ? string.Empty : _inputs.Dequeue();
        }
    }
}
