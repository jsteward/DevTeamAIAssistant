using System;

namespace DevTeamAIAssistant.Models;

public class RetrospectiveReport
{
    public string OverallSentiment { get; set; } = string.Empty;
    public List<string> KeyThemes { get; set; } = new();
    public List<ActionItem> ActionItems { get; set; } = new();
    public List<string> Concerns { get; set; } = new();
    public List<string> Wins { get; set; } = new();
    public string ManagerRecommendation { get; set; } = string.Empty;
}
