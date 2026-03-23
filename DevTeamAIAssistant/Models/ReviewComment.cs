using System;

namespace DevTeamAIAssistant.Models;

public class ReviewComment
{
    public string Category { get; set; } = string.Empty; // Architecture, Performance, Security, etc.
    public string Severity { get; set; } = string.Empty; // Critical, High, Medium, Low
    public string Issue { get; set; } = string.Empty;
    public string Suggestion { get; set; } = string.Empty;
    public int LineNumber { get; set; }
}
