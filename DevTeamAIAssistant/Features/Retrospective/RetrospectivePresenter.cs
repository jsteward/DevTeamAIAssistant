using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.Retrospective;

public class RetrospectivePresenter : IAnalyzerPresenter<RetrospectiveAnalyzerResponse>
{
    private const int SeparatorWidth = 60;
    private readonly IConsoleWriter _writer;

    public RetrospectivePresenter(IConsoleWriter writer)
    {
        _writer = writer;
    }

    public void Display(RetrospectiveAnalyzerResponse response)
    {
        var report = response.Report;

        _writer.WriteLine("\n" + new string('=', SeparatorWidth));
        _writer.WriteLine("SPRINT RETROSPECTIVE ANALYSIS");
        _writer.WriteLine(new string('=', SeparatorWidth));

        _writer.WriteLine($"\nOverall Sentiment: {report.OverallSentiment.ToUpper()}");

        _writer.WriteLine("\nKey Themes:");
        foreach (var theme in report.KeyThemes)
            _writer.WriteLine($"  • {theme}");

        _writer.WriteLine("\nWins:");
        foreach (var win in report.Wins)
            _writer.WriteLine($"  • {win}");

        _writer.WriteLine("\nConcerns:");
        foreach (var concern in report.Concerns)
            _writer.WriteLine($"  • {concern}");

        _writer.WriteLine("\nAction Items:");
        foreach (var item in report.ActionItems)
        {
            _writer.WriteLine($"  [{item.Priority.ToUpper()}] {item.Description}");
            _writer.WriteLine($"      Owner: {item.Owner} | Effort: {item.EstimatedEffortDays} days");
        }

        _writer.WriteLine("\nManager Recommendation:");
        _writer.WriteLine($"  {report.ManagerRecommendation}");

        _writer.WriteLine("\n" + new string('=', SeparatorWidth) + "\n");
    }
}
