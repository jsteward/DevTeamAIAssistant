using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.Presenters;

public class CodeReviewPresenter : IAnalyzerPresenter<CodeReviewAnalyzerResponse>
{
    public void Display(CodeReviewAnalyzerResponse response)
    {
        var review = response.Review;

        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("CODE REVIEW ANALYSIS");
        Console.WriteLine(new string('=', 60));

        Console.WriteLine($"\nQuality Score: {review.QualityScore}/10");
        Console.WriteLine($"\nOverall Assessment:\n  {review.OverallAssessment}");

        if (review.SecurityConcerns.Any())
        {
            Console.WriteLine("\nSecurity Concerns:");
            foreach (var concern in review.SecurityConcerns)
                Console.WriteLine($"  {concern}");
        }

        if (review.BestPractices.Any())
        {
            Console.WriteLine("\nBest Practices Observed:");
            foreach (var practice in review.BestPractices)
                Console.WriteLine($"  • {practice}");
        }

        Console.WriteLine("\nReview Comments:");
        foreach (var group in review.Comments.GroupBy(c => c.Severity).OrderBy(g => GetSeverityOrder(g.Key)))
        {
            Console.WriteLine($"\n  [{group.Key.ToUpper()}]");
            foreach (var comment in group)
            {
                Console.WriteLine($"    {comment.Category}: {comment.Issue}");
                Console.WriteLine($"    {comment.Suggestion}");
                if (comment.LineNumber > 0)
                    Console.WriteLine($"    Line {comment.LineNumber}");
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
