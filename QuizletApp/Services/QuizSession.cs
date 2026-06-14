using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class QuizSession
{
    public QuizSession(IReadOnlyList<Flashcard> cards)
    {
        Cards = cards;
    }

    public IReadOnlyList<Flashcard> Cards { get; }
    public int CurrentIndex { get; private set; }

    public bool IsComplete => CurrentIndex >= Cards.Count;

    public Flashcard CurrentCard => Cards[CurrentIndex];

    public bool Advance()
    {
        if (IsComplete)
        {
            return false;
        }

        CurrentIndex++;
        return !IsComplete;
    }
}
