using System;
using System.Reflection.Emit;
using DevTeamAIAssistant.Models;

namespace DevTeamAIAssistant.Response;

public class CodeReviewAnalyzerResponse : IAnalyzerResponse
{
    public CodeReviewAnalyzerResponse()
    {
        Review = new CodeReviewResult();
    }    

    public CodeReviewResult Review { get; set; }
    public void Display()
    {
        if(Review == null)
        {
            Console.WriteLine("No review data to display.");
            return;
        }
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("CODE REVIEW ANALYSIS");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine($"\n📊 Quality Score: {Review.QualityScore}/10");
        Console.WriteLine($"\n📝 Overall Assessment:\n  {Review.OverallAssessment}");
        
        if (Review.SecurityConcerns.Any())
        {
            Console.WriteLine("\n🔒 Security Concerns:");
            foreach (var concern in Review.SecurityConcerns)
            {
                Console.WriteLine($"  ⚠️  {concern}");
            }
        }
        
        if (Review.BestPractices.Any())
        {
            Console.WriteLine("\n✅ Best Practices Observed:");
            foreach (var practice in Review.BestPractices)
            {
                Console.WriteLine($"  • {practice}");
            }
        }
        
        Console.WriteLine("\n💬 Review Comments:");
        var groupedComments = Review.Comments.GroupBy(c => c.Severity);
        
        foreach (var group in groupedComments.OrderBy(g => GetSeverityOrder(g.Key)))
        {
            Console.WriteLine($"\n  [{group.Key.ToUpper()}]");
            foreach (var comment in group)
            {
                Console.WriteLine($"    {comment.Category}: {comment.Issue}");
                Console.WriteLine($"    💡 {comment.Suggestion}");
                if (comment.LineNumber > 0)
                {
                    Console.WriteLine($"    📍 Line {comment.LineNumber}");
                }
                Console.WriteLine();
            }
        }
        
        Console.WriteLine(new string('=', 60) + "\n");
    }

    private int GetSeverityOrder(string severity)
    {
        return severity.ToLower() switch
        {
            "critical" => 1,
            "high" => 2,
            "medium" => 3,
            "low" => 4,
            _ => 5
        };
    }
}
