using System;

namespace DevTeamAIAssistant.Models;

public class ActionItem
{
    public string Description { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;
    public int EstimatedEffortDays { get; set; }
}
