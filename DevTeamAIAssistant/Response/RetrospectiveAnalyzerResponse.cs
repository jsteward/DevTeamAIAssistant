using System;
using DevTeamAIAssistant.Models;

namespace DevTeamAIAssistant.Response;

public class RetrospectiveAnalyzerResponse : IAnalyzerResponse
{
    public RetrospectiveReport Report { get; set; }
    public RetrospectiveAnalyzerResponse()
    {
        Report = new RetrospectiveReport();
    }
    public void Display()
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("SPRINT RETROSPECTIVE ANALYSIS");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine($"\n📊 Overall Sentiment: {Report.OverallSentiment.ToUpper()}");
        
        Console.WriteLine("\n🎯 Key Themes:");
        foreach (var theme in Report.KeyThemes)
        {
            Console.WriteLine($"  • {theme}");
        }
        
        Console.WriteLine("\n✅ Wins:");
        foreach (var win in Report.Wins)
        {
            Console.WriteLine($"  • {win}");
        }
        
        Console.WriteLine("\n⚠️  Concerns:");
        foreach (var concern in Report.Concerns)
        {
            Console.WriteLine($"  • {concern}");
        }
        
        Console.WriteLine("\n📋 Action Items:");
        foreach (var item in Report.ActionItems)
        {
            Console.WriteLine($"  [{item.Priority.ToUpper()}] {item.Description}");
            Console.WriteLine($"      Owner: {item.Owner} | Effort: {item.EstimatedEffortDays} days");
        }
        
        Console.WriteLine("\n💡 Manager Recommendation:");
        Console.WriteLine($"  {Report.ManagerRecommendation}");
        
        Console.WriteLine("\n" + new string('=', 60) + "\n");
    }
}
