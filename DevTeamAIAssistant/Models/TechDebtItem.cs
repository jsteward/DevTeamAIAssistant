namespace DevTeamAIAssistant.Models;

public class TechDebtAnalysis
{
    public List<PrioritizedDebtItem> PrioritizedItems { get; set; } = new();
    public string OverallRecommendation { get; set; } = string.Empty;
    public string RiskAssessment { get; set; } = string.Empty;
    public int TotalEstimatedDays { get; set; }
}
