namespace DevTeamAIAssistant.Models;

public enum SeverityLevel
{
    Critical = 1,
    High = 2,
    Medium = 3,
    Low = 4,
    Unknown = 5
}

public static class SeverityLevelExtensions
{
    public static SeverityLevel Parse(string severity) => severity.ToLower() switch
    {
        "critical" => SeverityLevel.Critical,
        "high"     => SeverityLevel.High,
        "medium"   => SeverityLevel.Medium,
        "low"      => SeverityLevel.Low,
        _          => SeverityLevel.Unknown
    };
}
