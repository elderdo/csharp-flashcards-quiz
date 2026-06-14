using System.Text.Json;
using QuizletApp.Models;

namespace QuizletApp.Services;

public sealed class QuizSettingsProvider(string[] args) : IQuizSettingsProvider
{
    public QuizSettings LoadSettings()
    {
        var path = ResolveSettingsPath();
        if (!File.Exists(path))
        {
            return new QuizSettings();
        }

        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<QuizSettings>(json) ?? new QuizSettings();
        }
        catch
        {
            Console.WriteLine("Could not read quizsettings.json. Using defaults.");
            return new QuizSettings();
        }
    }

    private string ResolveSettingsPath()
    {
        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.Equals("--settings", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
            {
                return ResolveRelativeOrAbsolute(args[i + 1]);
            }

            const string prefix = "--settings=";
            if (arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                return ResolveRelativeOrAbsolute(arg[prefix.Length..]);
            }
        }

        return Path.Combine(Directory.GetCurrentDirectory(), "quizsettings.json");
    }

    private static string ResolveRelativeOrAbsolute(string path)
    {
        return Path.IsPathRooted(path)
            ? path
            : Path.Combine(Directory.GetCurrentDirectory(), path);
    }
}
