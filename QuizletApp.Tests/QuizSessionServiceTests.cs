using QuizletApp.Models;
using QuizletApp.Services;

namespace QuizletApp.Tests;

public class QuizSessionServiceTests
{
    [Fact]
    public void StartSession_WithNoCards_ThrowsArgumentException()
    {
        var service = new QuizSessionService();

        Assert.Throws<ArgumentException>(() => service.StartSession([], shuffle: false));
    }

    [Fact]
    public void StartSession_WithoutShuffle_PreservesOrderAndProgresses()
    {
        var service = new QuizSessionService();
        var cards = new[]
        {
            new Flashcard("Q1", "A1"),
            new Flashcard("Q2", "A2")
        };

        var session = service.StartSession(cards, shuffle: false);

        Assert.True(service.TryGetPrompt(session, out var first));
        Assert.Equal(1, first.CardNumber);
        Assert.Equal("Q1", first.Question);
        Assert.Equal("A1", service.RevealAnswer(session));

        Assert.True(service.MoveNext(session));
        Assert.True(service.TryGetPrompt(session, out var second));
        Assert.Equal(2, second.CardNumber);
        Assert.Equal("Q2", second.Question);

        Assert.False(service.MoveNext(session));
        Assert.False(service.TryGetPrompt(session, out _));
    }

    [Fact]
    public void RevealAnswer_AfterCompletion_ThrowsInvalidOperationException()
    {
        var service = new QuizSessionService();
        var session = service.StartSession([new Flashcard("Q1", "A1")], shuffle: false);

        service.MoveNext(session);

        Assert.Throws<InvalidOperationException>(() => service.RevealAnswer(session));
    }
}
