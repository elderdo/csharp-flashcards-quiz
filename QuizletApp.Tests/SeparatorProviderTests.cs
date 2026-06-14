using QuizletApp.Models;
using QuizletApp.Services;

namespace QuizletApp.Tests;

public class SeparatorProviderTests
{
    [Fact]
    public void BuildSeparators_IncludesRequestedDefaultSeparators()
    {
        var provider = new SeparatorProvider([]);

        var separators = provider.BuildSeparators(new QuizSettings());

        Assert.Contains("\t", separators);
        Assert.Contains(",", separators);
        Assert.Contains(";", separators);
        Assert.Contains("|", separators);
        Assert.Contains(":", separators);
        Assert.Contains("~", separators);
        Assert.Contains("^", separators);
    }

    [Fact]
    public void BuildSeparators_AcceptsCustomFromConfigAndCommandLine()
    {
        var provider = new SeparatorProvider(["--separator", "@@@", "--separator=TAB"]);

        var separators = provider.BuildSeparators(new QuizSettings
        {
            CustomSeparators = ["###", "@@@"]
        });

        Assert.Contains("###", separators);
        Assert.Contains("@@@", separators);
        Assert.Contains("\t", separators);
        Assert.Equal(1, separators.Count(s => s == "@@@"));
    }

    [Fact]
    public void BuildSeparators_OrdersLongestSeparatorFirst()
    {
        var provider = new SeparatorProvider([]);
        var separators = provider.BuildSeparators(new QuizSettings { CustomSeparators = ["#", "###"] });
        var materialized = separators.ToList();

        Assert.True(materialized.IndexOf("###") < materialized.IndexOf("#"));
    }
}
