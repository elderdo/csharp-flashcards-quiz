# Quizlet Console App (C#)

This workspace contains a simple C# console app that reads Quizlet-style flashcards from text files, randomizes questions, and reveals answers after you press Enter.

## Projects

- QuizletApp: main app
- QuizletApp.Tests: unit and integration tests

## Architecture Notes (GUI Ready)

The app is split into reusable services so you can later add WinForms, WPF, or WinUI without rewriting core quiz logic.

- QuizCatalogService: resolves settings, separators, quiz files, and card loading.
- QuizSessionService: quiz session state machine (prompt, reveal answer, move next).
- IUserInteraction + ConsoleUserInteraction: console adapter layer.

A GUI can use QuizCatalogService and QuizSessionService directly, and replace IUserInteraction with view-model or window logic.

## Prerequisites

- .NET 10 SDK installed

## Initial Setup

From the workspace root:

1. dotnet restore QuizletApp/QuizletApp.csproj
2. dotnet restore QuizletApp.Tests/QuizletApp.Tests.csproj
3. dotnet test QuizletApp.Tests/QuizletApp.Tests.csproj

If step 3 passes, the app and quiz data are configured correctly.

## Run The App

From the workspace root:

1. cd QuizletApp
2. dotnet run

If you want to add command-line separators while running:

- dotnet run -- --separator ###
- dotnet run -- -s @@@
- dotnet run -- --separator=TAB

If you want to use a custom settings file:

- dotnet run -- --settings mysettings.json
- dotnet run -- --settings=C:/path/to/mysettings.json

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

You can also use custom tokens:

What is inversion of control?###The framework controls object creation and flow.

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

Tips:

- Keep DataDirectory as data unless you need a shared folder.
- Use CustomSeparators when your questions/answers frequently include commas.
- Prefer multi-character separators such as ### for readability.

## Create More Quizzes

1. Add a new text file under QuizletApp/data.
2. Put one flashcard per line using any enabled separator.
3. Save the file with .txt, .tsv, or .csv extension.
4. Run the app and select the file from the menu.

Recommended authoring rules:

- Keep each line as one card only.
- Avoid empty question or answer text.
- Use # at the beginning of a line for comments.
- If your separator appears in normal sentence text often, switch to a custom token (for example ###).

Suggested naming style:

- csharp-generics-basics.txt
- csharp-async-await.txt
- csharp-pattern-matching.txt

## Validate Quiz Files

Run this to validate all quiz files parse correctly:

1. dotnet test QuizletApp.Tests/QuizletApp.Tests.csproj --filter QuizDataFilesTests

This fails fast when any quiz line has an invalid format.

## Existing C# Quiz Sets

- csharp-operators.txt
- csharp-nullable-variables.txt
- csharp-linq-options.txt
- csharp-linq-syntax-patterns.txt
- csharp-async-await-pitfalls.txt
- csharp-collections-dictionaries.txt
- csharp-unit-testing.txt
- csharp-mvvm.txt
- csharp-oop-solid-principles.txt
- database-normalization-rules.txt
- database-normalization-scenarios.txt
- tsql-basics.txt
- tsql-optimization.txt
- sql-server-ssms-plans-runtime.txt
- entity-framework-sql-server.txt

Each quiz file is intentionally focused to keep study sessions short.

## Suggested Study Order

1. csharp-operators.txt (10-15 minutes)
2. csharp-nullable-variables.txt (10-15 minutes)
3. csharp-linq-options.txt (15-20 minutes)
4. csharp-linq-syntax-patterns.txt (15-20 minutes)
5. csharp-collections-dictionaries.txt (10-15 minutes)
6. csharp-async-await-pitfalls.txt (15-20 minutes)
7. csharp-unit-testing.txt (15-20 minutes)
8. csharp-oop-solid-principles.txt (15-20 minutes)
9. csharp-mvvm.txt (15-20 minutes)
10. database-normalization-rules.txt (15-20 minutes)
11. database-normalization-scenarios.txt (15-20 minutes)
12. tsql-basics.txt (15-20 minutes)
13. tsql-optimization.txt (20-25 minutes)
14. sql-server-ssms-plans-runtime.txt (20-25 minutes)
15. entity-framework-sql-server.txt (15-20 minutes)

## YouTube Learning References

Use these as quick-start references for each category.

- C# operators and nullable types: https://www.youtube.com/results?search_query=c%23+operators+nullable+reference+types
- LINQ options and syntax: https://www.youtube.com/results?search_query=c%23+linq+method+syntax+query+syntax
- Async/await pitfalls: https://www.youtube.com/results?search_query=c%23+async+await+pitfalls
- Collections and dictionaries: https://www.youtube.com/results?search_query=c%23+dictionary+hashset+list+best+practices
- Unit testing (xUnit): https://www.youtube.com/results?search_query=c%23+xunit+unit+testing
- OOP and SOLID principles: https://www.youtube.com/results?search_query=c%23+solid+principles+oop
- MVVM (WPF/WinUI): https://www.youtube.com/results?search_query=c%23+mvvm+wpf+winui
- Database normalization: https://www.youtube.com/results?search_query=database+normalization+1nf+2nf+3nf+bcnf+4nf+5nf
- T-SQL basics: https://www.youtube.com/results?search_query=t-sql+basics+sql+server
- SQL Server query optimization: https://www.youtube.com/results?search_query=sql+server+query+optimization+execution+plan
- SSMS execution plans and performance checks: https://www.youtube.com/results?search_query=ssms+actual+execution+plan+statistics+io+time
- Entity Framework with SQL Server: https://www.youtube.com/results?search_query=entity+framework+core+sql+server+performance

## Tests

Run all tests from the workspace root:

1. dotnet test QuizletApp.Tests/QuizletApp.Tests.csproj

Run build for the app only:

1. dotnet build QuizletApp/QuizletApp.csproj

## Notes

- Lines beginning with # are treated as comments.
- Empty lines are ignored.
- The app parses the first matching separator found in each line.
- Quiz data files are validated by tests to ensure card lines parse correctly.
