using DevTeamAIAssistant.Models;

namespace DevTeamAIAssistant.Response;

public class CodeReviewAnalyzerResponse : IAnalyzerResponse
{
    public CodeReviewResult Review { get; set; }

    public CodeReviewAnalyzerResponse()
    {
        Review = new CodeReviewResult();
    }
}
