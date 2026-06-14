# Quizlet Console App (C#)

This workspace contains a simple C# console app that reads Quizlet-style flashcards from text files, randomizes questions, and reveals answers after you press Enter.

## Projects

- QuizletApp: main app
- QuizletApp.Tests: unit and integration tests

## Prerequisites

- .NET 10 SDK installed

## Run The App

From the workspace root:

1. cd QuizletApp
2. dotnet run

If you want to add command-line separators while running:

- dotnet run -- --separator ###
- dotnet run -- -s @@@
- dotnet run -- --separator=TAB

## Where Quiz Files Live

By default, quiz files are loaded from:

- QuizletApp/data

Supported file types:

- .txt
- .tsv
- .csv

## Card Format

Each card is one line:

question<separator>answer

Example with TAB separator:

What does ?? do in C#? Null-coalescing operator; returns left if not null, otherwise right.

Built-in separators:

- TAB
- ,
- ;
- |
- :
- ~
- ^

Custom multi-character separators are supported (for example ###).

## Configuration

App settings file:

- QuizletApp/quizsettings.json

Example:

{
"DataDirectory": "data",
"CustomSeparators": ["###"]
}

DataDirectory can be relative to the app folder or an absolute path.

You can also choose a different settings file at runtime:

- dotnet run -- --settings mysettings.json
- dotnet run -- --settings=C:/path/to/mysettings.json

## Create More Quizzes

1. Add a new text file under QuizletApp/data.
2. Put one flashcard per line using any enabled separator.
3. Save the file with .txt, .tsv, or .csv extension.
4. Run the app and select the file from the menu.

Suggested naming style:

- csharp-generics-basics.txt
- csharp-async-await.txt
- csharp-pattern-matching.txt

## Existing C# Quiz Sets

- csharp-operators.txt
- csharp-nullable-variables.txt
- csharp-linq-options.txt
- csharp-linq-syntax-patterns.txt
- csharp-async-await-pitfalls.txt
- csharp-collections-dictionaries.txt

## Tests

Run all tests from the workspace root:

1. dotnet test QuizletApp.Tests/QuizletApp.Tests.csproj

Run build for the app only:

1. dotnet build QuizletApp/QuizletApp.csproj

## Notes

- Lines beginning with # are treated as comments.
- Empty lines are ignored.
- The app parses the first matching separator found in each line.
