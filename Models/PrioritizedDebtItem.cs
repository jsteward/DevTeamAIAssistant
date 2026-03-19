namespace DevTeamAIAssistant.Models;

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