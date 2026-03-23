using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Features;

public class RetrospectiveAnalyzer
{
    private readonly IClaudeService _claudeService;

    public RetrospectiveAnalyzer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<RetrospectiveReport> AnalyzeRetrospectiveAsync(string retrospectiveNotes)
    {
        var prompt = @"You are an expert Agile coach analyzing a sprint retrospective.

Analyze the following retrospective notes and provide a structured report in JSON format:

{
  ""overallSentiment"": ""positive/neutral/negative"",
  ""keyThemes"": [""theme1"", ""theme2"", ""theme3""],
  ""actionItems"": [
    {
      ""description"": ""specific action"",
      ""priority"": ""high/medium/low"",
      ""owner"": ""suggested role/person"",
      ""estimatedEffortDays"": 3
    }
  ],
  ""concerns"": [""concern1"", ""concern2""],
  ""wins"": [""win1"", ""win2""],
  ""managerRecommendation"": ""specific advice for the development manager""
}

Focus on actionable insights and realistic recommendations.";

        return await _claudeService.AnalyzeStructuredAsync<RetrospectiveReport>(
            prompt, 
            retrospectiveNotes
        );
    }

    public void DisplayReport(RetrospectiveReport report)
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("SPRINT RETROSPECTIVE ANALYSIS");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine($"\n📊 Overall Sentiment: {report.OverallSentiment.ToUpper()}");
        
        Console.WriteLine("\n🎯 Key Themes:");
        foreach (var theme in report.KeyThemes)
        {
            Console.WriteLine($"  • {theme}");
        }
        
        Console.WriteLine("\n✅ Wins:");
        foreach (var win in report.Wins)
        {
            Console.WriteLine($"  • {win}");
        }
        
        Console.WriteLine("\n⚠️  Concerns:");
        foreach (var concern in report.Concerns)
        {
            Console.WriteLine($"  • {concern}");
        }
        
        Console.WriteLine("\n📋 Action Items:");
        foreach (var item in report.ActionItems)
        {
            Console.WriteLine($"  [{item.Priority.ToUpper()}] {item.Description}");
            Console.WriteLine($"      Owner: {item.Owner} | Effort: {item.EstimatedEffortDays} days");
        }
        
        Console.WriteLine("\n💡 Manager Recommendation:");
        Console.WriteLine($"  {report.ManagerRecommendation}");
        
        Console.WriteLine("\n" + new string('=', 60) + "\n");
    }
}