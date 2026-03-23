using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Models;

public class CodeReviewResult
{
    public string OverallAssessment { get; set; } = string.Empty;
    public List<ReviewComment> Comments { get; set; } = new();
    public List<string> SecurityConcerns { get; set; } = new();
    public List<string> BestPractices { get; set; } = new();
    public int QualityScore { get; set; } // 1-10
}

