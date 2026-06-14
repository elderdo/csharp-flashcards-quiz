using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class SeparatorProvider(string[] args) : ISeparatorProvider
{
    public IReadOnlyList<string> BuildSeparators(QuizSettings settings)
    {
        var defaults = new[] { "\t", ",", ";", "|", ":", "~", "^" };
        var configSeparators = settings.CustomSeparators ?? [];
        var cliSeparators = ParseCliSeparators();

        return defaults
            .Concat(configSeparators)
            .Concat(cliSeparators)
            .Select(NormalizeSeparator)
            .Where(s => s == "\t" || !string.IsNullOrWhiteSpace(s))
            .Distinct(StringComparer.Ordinal)
            .OrderByDescending(s => s.Length)
            .ToList();
    }

    private IEnumerable<string> ParseCliSeparators()
    {
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.Equals("--separator", StringComparison.OrdinalIgnoreCase)
                || arg.Equals("-s", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 < args.Length)
                {
                    yield return args[i + 1];
                    i++;
                }

                continue;
            }

            const string prefix = "--separator=";
            if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                yield return arg[prefix.Length..];
            }
        }
    }

    private static string NormalizeSeparator(string separator)
    {
        return separator.Trim() switch
        {
            "TAB" => "\t",
            "\\t" => "\t",
            _ => separator
        };
    }

    public static string Display(string separator)
    {
        return separator == "\t" ? "TAB" : separator;
    }
}
