using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Features;

public class TechDebtPrioritizer
{
    private readonly IClaudeService _claudeService;

    public TechDebtPrioritizer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<TechDebtAnalysis> PrioritizeDebtAsync(List<string> debtItems)
    {
        var itemsList = string.Join("\n", debtItems.Select((item, i) => $"{i + 1}. {item}"));
        
        var prompt = @"You are a technical architect helping prioritize technical debt.

Analyze these technical debt items and prioritize them based on:
- Business impact (revenue, customer satisfaction, operational efficiency)
- Risk (security, stability, scalability)
- Effort required
- Dependencies
- ROI (Return on Investment)

Return JSON in this exact format:
{
  ""prioritizedItems"": [
    {
      ""description"": ""item description"",
      ""priority"": 8,
      ""impact"": ""High/Medium/Low"",
      ""effort"": ""High/Medium/Low"",
      ""roiScore"": 85,
      ""reasoning"": ""why this priority"",
      ""estimatedDays"": 5,
      ""dependencies"": [""item 1"", ""item 2""]
    }
  ],
  ""overallRecommendation"": ""strategic recommendation"",
  ""riskAssessment"": ""overall risk summary"",
  ""totalEstimatedDays"": 20
}

Sort by priority (highest first). Be realistic about effort estimates.";

        return await _claudeService.AnalyzeStructuredAsync<TechDebtAnalysis>(
            prompt,
            itemsList
        );
    }

    public void DisplayAnalysis(TechDebtAnalysis analysis)
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("TECHNICAL DEBT PRIORITIZATION");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine($"\n⚠️  Risk Assessment:\n  {analysis.RiskAssessment}");
        Console.WriteLine($"\n📊 Total Estimated Effort: {analysis.TotalEstimatedDays} days");
        
        Console.WriteLine("\n🎯 Prioritized Backlog:");
        Console.WriteLine(new string('-', 60));
        
        foreach (var item in analysis.PrioritizedItems.OrderByDescending(i => i.Priority))
        {
            var priorityIndicator = item.Priority >= 8 ? "🔴" : 
                                   item.Priority >= 5 ? "🟡" : "🟢";
            
            Console.WriteLine($"\n{priorityIndicator} Priority: {item.Priority}/10 | Impact: {item.Impact} | Effort: {item.Effort}");
            Console.WriteLine($"   ROI Score: {item.RoiScore}/100");
            Console.WriteLine($"   {item.Description}");
            Console.WriteLine($"   💭 {item.Reasoning}");
            Console.WriteLine($"   ⏱️  Estimated: {item.EstimatedDays} days");
            
            if (item.Dependencies.Any())
            {
                Console.WriteLine($"   🔗 Dependencies: {string.Join(", ", item.Dependencies)}");
            }
        }
        
        Console.WriteLine("\n💡 Strategic Recommendation:");
        Console.WriteLine($"  {analysis.OverallRecommendation}");
        
        Console.WriteLine("\n" + new string('=', 60) + "\n");
    }
}