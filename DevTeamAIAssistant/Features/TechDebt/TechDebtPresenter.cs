using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.TechDebt;

public class TechDebtPresenter : IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse>
{
    private const int SeparatorWidth = 60;
    private const int HighPriorityThreshold = 8;
    private const int MediumPriorityThreshold = 5;
    private readonly IConsoleWriter _writer;

    public TechDebtPresenter(IConsoleWriter writer)
    {
        _writer = writer;
    }

    public void Display(TechDebtPriorityAnalyzerResponse response)
    {
        var analysis = response.Report;

        _writer.WriteLine("\n" + new string('=', SeparatorWidth));
        _writer.WriteLine("TECHNICAL DEBT PRIORITIZATION");
        _writer.WriteLine(new string('=', SeparatorWidth));

        _writer.WriteLine($"\nRisk Assessment:\n  {analysis.RiskAssessment}");
        _writer.WriteLine($"\nTotal Estimated Effort: {analysis.TotalEstimatedDays} days");

        _writer.WriteLine("\nPrioritized Backlog:");
        _writer.WriteLine(new string('-', SeparatorWidth));

        foreach (var item in analysis.PrioritizedItems.OrderByDescending(i => i.Priority))
        {
            var indicator = item.Priority >= HighPriorityThreshold ? "[HIGH]" : item.Priority >= MediumPriorityThreshold ? "[MED]" : "[LOW]";
            _writer.WriteLine($"\n{indicator} Priority: {item.Priority}/10 | Impact: {item.Impact} | Effort: {item.Effort}");
            _writer.WriteLine($"   ROI Score: {item.RoiScore}/100");
            _writer.WriteLine($"   {item.Description}");
            _writer.WriteLine($"   {item.Reasoning}");
            _writer.WriteLine($"   Estimated: {item.EstimatedDays} days");

            if (item.Dependencies.Any())
                _writer.WriteLine($"   Dependencies: {string.Join(", ", item.Dependencies)}");
        }

        _writer.WriteLine("\nStrategic Recommendation:");
        _writer.WriteLine($"  {analysis.OverallRecommendation}");

        _writer.WriteLine("\n" + new string('=', SeparatorWidth) + "\n");
    }
}
