namespace QuizletApp.Models;

public readonly record struct QuizPrompt(int CardNumber, int TotalCards, string Question);
