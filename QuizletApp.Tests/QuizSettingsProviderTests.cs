using QuizletApp.Services;

namespace QuizletApp.Tests;

public class QuizSettingsProviderTests
{
    [Fact]
    public void LoadSettings_UsesCustomSettingsPathFromArguments()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tempFile, "{\"DataDirectory\":\"my-data\",\"CustomSeparators\":[\"###\"]}");

            var provider = new QuizSettingsProvider(["--settings", tempFile]);

            var settings = provider.LoadSettings();

            Assert.Equal("my-data", settings.DataDirectory);
            Assert.Contains("###", settings.CustomSeparators ?? []);
        }
        finally
        {
            File.Delete(tempFile);
        }
    }
}
