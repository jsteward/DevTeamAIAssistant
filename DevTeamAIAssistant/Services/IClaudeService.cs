namespace DevTeamAIAssistant.Services;

public interface IClaudeService
{
    Task<string> AnalyzeAsync(string prompt, string context);
    Task<T> AnalyzeStructuredAsync<T>(string prompt, string context) where T : class;
}