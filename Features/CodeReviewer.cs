using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Features;

public class CodeReviewer
{
    private readonly IClaudeService _claudeService;

    public CodeReviewer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<CodeReviewResult> ReviewCodeAsync(string code, string? context = null)
    {
        var prompt = $@"You are a senior software architect reviewing C# code.

Provide a thorough code review in JSON format focusing on:
- Architecture and design patterns
- Security vulnerabilities
- Performance considerations
- .NET best practices
- SOLID principles
- Potential bugs

{(context != null ? $"Additional context: {context}" : "")}

Return JSON in this exact format:
{{
  ""overallAssessment"": ""summary of code quality"",
  ""comments"": [
    {{
      ""category"": ""Architecture/Performance/Security/Style/Bug"",
      ""severity"": ""Critical/High/Medium/Low"",
      ""issue"": ""what's wrong"",
      ""suggestion"": ""how to fix it"",
      ""lineNumber"": 0
    }}
  ],
  ""securityConcerns"": [""concern1"", ""concern2""],
  ""bestPractices"": [""good practice observed""],
  ""qualityScore"": 7
}}";

        return await _claudeService.AnalyzeStructuredAsync<CodeReviewResult>(
            prompt,
            code
        );
    }

    public void DisplayReview(CodeReviewResult review)
    {
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("CODE REVIEW ANALYSIS");
        Console.WriteLine(new string('=', 60));
        
        Console.WriteLine($"\n📊 Quality Score: {review.QualityScore}/10");
        Console.WriteLine($"\n📝 Overall Assessment:\n  {review.OverallAssessment}");
        
        if (review.SecurityConcerns.Any())
        {
            Console.WriteLine("\n🔒 Security Concerns:");
            foreach (var concern in review.SecurityConcerns)
            {
                Console.WriteLine($"  ⚠️  {concern}");
            }
        }
        
        if (review.BestPractices.Any())
        {
            Console.WriteLine("\n✅ Best Practices Observed:");
            foreach (var practice in review.BestPractices)
            {
                Console.WriteLine($"  • {practice}");
            }
        }
        
        Console.WriteLine("\n💬 Review Comments:");
        var groupedComments = review.Comments.GroupBy(c => c.Severity);
        
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

    private static int GetSeverityOrder(string severity) => severity.ToLower() switch
    {
        "critical" => 1,
        "high" => 2,
        "medium" => 3,
        "low" => 4,
        _ => 5
    };
}