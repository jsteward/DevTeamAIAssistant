using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.Presenters;

public class TechDebtPresenter : IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse>
{
    public void Display(TechDebtPriorityAnalyzerResponse response)
    {
        var analysis = response.Report;

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("TECHNICAL DEBT PRIORITIZATION");
        Console.WriteLine(new string('=', 60));

        Console.WriteLine($"\nRisk Assessment:\n  {analysis.RiskAssessment}");
        Console.WriteLine($"\nTotal Estimated Effort: {analysis.TotalEstimatedDays} days");

        Console.WriteLine("\nPrioritized Backlog:");
        Console.WriteLine(new string('-', 60));

        foreach (var item in analysis.PrioritizedItems.OrderByDescending(i => i.Priority))
        {
            var indicator = item.Priority >= 8 ? "[HIGH]" : item.Priority >= 5 ? "[MED]" : "[LOW]";
            Console.WriteLine($"\n{indicator} Priority: {item.Priority}/10 | Impact: {item.Impact} | Effort: {item.Effort}");
            Console.WriteLine($"   ROI Score: {item.RoiScore}/100");
            Console.WriteLine($"   {item.Description}");
            Console.WriteLine($"   {item.Reasoning}");
            Console.WriteLine($"   Estimated: {item.EstimatedDays} days");

            if (item.Dependencies.Any())
                Console.WriteLine($"   Dependencies: {string.Join(", ", item.Dependencies)}");
        }

        Console.WriteLine("\nStrategic Recommendation:");
        Console.WriteLine($"  {analysis.OverallRecommendation}");

        Console.WriteLine("\n" + new string('=', 60) + "\n");
    }
}
