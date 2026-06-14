using System.Globalization;
using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class QuizApplication(IQuizCatalogService catalogService, IUserInteraction ui)
{
    public Task RunAsync()
    {
        var dataDirectory = catalogService.GetDataDirectory();
        Directory.CreateDirectory(dataDirectory);

        var separators = catalogService.GetSeparators();

        ui.WriteLine("Quizlet-style Flashcards");
        ui.WriteLine($"Data folder: {dataDirectory}");
        ui.WriteLine("Expected line format: question<separator>answer");
        ui.WriteLine($"Separators enabled: {string.Join(", ", separators.Select(SeparatorProvider.Display))}");
        ui.WriteLine();

        var quizFiles = catalogService.GetQuizFiles(dataDirectory);

        if (quizFiles.Count == 0)
        {
            ui.WriteLine("No quiz files found in the data folder.");
            ui.WriteLine("Add a .txt, .tsv, or .csv file using one card per line.");
            return Task.CompletedTask;
        }

        var selectedFile = SelectQuizFile(quizFiles);
        var cards = catalogService.LoadCards(selectedFile, separators);

        if (cards.Count == 0)
        {
            ui.WriteLine("No valid cards were found in the selected file.");
            ui.WriteLine("Make sure each non-empty line is question<separator>answer.");
            return Task.CompletedTask;
        }

        ui.WriteLine();
        ui.WriteLine($"Loaded {cards.Count} cards from {Path.GetFileName(selectedFile)}");
        ui.WriteLine();

        RunQuiz(cards);
        return Task.CompletedTask;
    }

    private string SelectQuizFile(IReadOnlyList<string> quizFiles)
    {
        if (quizFiles.Count == 1)
        {
            return quizFiles[0];
        }

        ui.WriteLine("Select a quiz file:");
        for (var i = 0; i < quizFiles.Count; i++)
        {
            ui.WriteLine($"{i + 1}. {Path.GetFileName(quizFiles[i])}");
        }

        while (true)
        {
            ui.Write("Enter file number: ");
            var input = ui.ReadLine();

            if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var selection)
                && selection >= 1
                && selection <= quizFiles.Count)
            {
                return quizFiles[selection - 1];
            }

            ui.WriteLine("Invalid selection. Try again.");
        }
    }

    private void RunQuiz(IReadOnlyList<Flashcard> cards)
    {
        while (true)
        {
            var randomized = cards
                .OrderBy(_ => Random.Shared.Next())
                .ToList();

            ui.WriteLine("Press Enter to reveal each answer. Type q and press Enter to quit.");
            ui.WriteLine();

            for (var i = 0; i < randomized.Count; i++)
            {
                var card = randomized[i];
                ui.WriteLine($"Card {i + 1}/{randomized.Count}");
                ui.WriteLine($"Q: {card.Question}");
                ui.Write("Reveal answer (Enter/q): ");

                var revealInput = ui.ReadLine();
                if (string.Equals(revealInput, "q", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                ui.WriteLine($"A: {card.Answer}");
                ui.WriteLine();
            }

            ui.Write("Round complete. Run again with a new random order? (y/n): ");
            var repeatInput = ui.ReadLine();
            if (!string.Equals(repeatInput, "y", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            ui.WriteLine();
        }
    }
}
