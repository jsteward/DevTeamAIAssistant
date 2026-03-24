using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.Presenters;

public class RetrospectivePresenter : IAnalyzerPresenter<RetrospectiveAnalyzerResponse>
{
    public void Display(RetrospectiveAnalyzerResponse response)
    {
        var report = response.Report;

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("SPRINT RETROSPECTIVE ANALYSIS");
        Console.WriteLine(new string('=', 60));

        Console.WriteLine($"\nOverall Sentiment: {report.OverallSentiment.ToUpper()}");

        Console.WriteLine("\nKey Themes:");
        foreach (var theme in report.KeyThemes)
            Console.WriteLine($"  • {theme}");

        Console.WriteLine("\nWins:");
        foreach (var win in report.Wins)
            Console.WriteLine($"  • {win}");

        Console.WriteLine("\nConcerns:");
        foreach (var concern in report.Concerns)
            Console.WriteLine($"  • {concern}");

        Console.WriteLine("\nAction Items:");
        foreach (var item in report.ActionItems)
        {
            Console.WriteLine($"  [{item.Priority.ToUpper()}] {item.Description}");
            Console.WriteLine($"      Owner: {item.Owner} | Effort: {item.EstimatedEffortDays} days");
        }

        Console.WriteLine("\nManager Recommendation:");
        Console.WriteLine($"  {report.ManagerRecommendation}");

        Console.WriteLine("\n" + new string('=', 60) + "\n");
    }
}
