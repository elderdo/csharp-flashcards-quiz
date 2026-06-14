using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class QuizSessionService : IQuizSessionService
{
    public QuizSession StartSession(IReadOnlyList<Flashcard> cards, bool shuffle = true)
    {
        if (cards.Count == 0)
        {
            throw new ArgumentException("A quiz session requires at least one card.", nameof(cards));
        }

        var orderedCards = shuffle
            ? cards.OrderBy(_ => Random.Shared.Next()).ToList()
            : cards.ToList();

        return new QuizSession(orderedCards);
    }

    public bool TryGetPrompt(QuizSession session, out QuizPrompt prompt)
    {
        if (session.IsComplete)
        {
            prompt = default;
            return false;
        }

        prompt = new QuizPrompt(
            session.CurrentIndex + 1,
            session.Cards.Count,
            session.CurrentCard.Question);

        return true;
    }

    public string RevealAnswer(QuizSession session)
    {
        if (session.IsComplete)
        {
            throw new InvalidOperationException("Cannot reveal an answer for a completed session.");
        }

        return session.CurrentCard.Answer;
    }

    public bool MoveNext(QuizSession session)
    {
        return session.Advance();
    }
}
