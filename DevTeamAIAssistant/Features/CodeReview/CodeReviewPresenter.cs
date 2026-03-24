using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features.CodeReview;

public class CodeReviewPresenter : IAnalyzerPresenter<CodeReviewAnalyzerResponse>
{
    private const int SeparatorWidth = 60;
    private readonly IConsoleWriter _writer;

    public CodeReviewPresenter(IConsoleWriter writer)
    {
        _writer = writer;
    }

    public void Display(CodeReviewAnalyzerResponse response)
    {
        var review = response.Review;

        _writer.WriteLine("\n" + new string('=', SeparatorWidth));
        _writer.WriteLine("CODE REVIEW ANALYSIS");
        _writer.WriteLine(new string('=', SeparatorWidth));

        _writer.WriteLine($"\nQuality Score: {review.QualityScore}/10");
        _writer.WriteLine($"\nOverall Assessment:\n  {review.OverallAssessment}");

        if (review.SecurityConcerns.Any())
        {
            _writer.WriteLine("\nSecurity Concerns:");
            foreach (var concern in review.SecurityConcerns)
                _writer.WriteLine($"  {concern}");
        }

        if (review.BestPractices.Any())
        {
            _writer.WriteLine("\nBest Practices Observed:");
            foreach (var practice in review.BestPractices)
                _writer.WriteLine($"  • {practice}");
        }

        _writer.WriteLine("\nReview Comments:");
        foreach (var group in review.Comments.GroupBy(c => c.Severity).OrderBy(g => SeverityLevelExtensions.Parse(g.Key)))
        {
            _writer.WriteLine($"\n  [{group.Key.ToUpper()}]");
            foreach (var comment in group)
            {
                _writer.WriteLine($"    {comment.Category}: {comment.Issue}");
                _writer.WriteLine($"    {comment.Suggestion}");
                if (comment.LineNumber > 0)
                    _writer.WriteLine($"    Line {comment.LineNumber}");
                _writer.WriteLine();
            }
        }

        _writer.WriteLine(new string('=', SeparatorWidth) + "\n");
    }
}
