namespace DevTeamAIAssistant.Features;

public interface IAnalyzerRunner
{
    string MenuKey { get; }
    string MenuLabel { get; }
    Task RunAsync();
}
