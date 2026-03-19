namespace DevTeamAIAssistant.Models;

public class TechDebtAnalysis
{
    public List<PrioritizedDebtItem> PrioritizedItems { get; set; } = new();
    public string OverallRecommendation { get; set; } = string.Empty;
    public string RiskAssessment { get; set; } = string.Empty;
    public int TotalEstimatedDays { get; set; }
}

public class PrioritizedDebtItem
{
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; } // 1-10
    public string Impact { get; set; } = string.Empty; // High/Medium/Low
    public string Effort { get; set; } = string.Empty; // High/Medium/Low
    public int RoiScore { get; set; } // 1-100
    public string Reasoning { get; set; } = string.Empty;
    public int EstimatedDays { get; set; }
    public List<string> Dependencies { get; set; } = new();
}