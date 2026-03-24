namespace DevTeamAIAssistant.Features;

public class AnalyzerFactory : IAnalyzerFactory
{
    private readonly Dictionary<string, IAnalyzerRunner> _runners;

    public AnalyzerFactory(IEnumerable<IAnalyzerRunner> runners)
    {
        _runners = runners.ToDictionary(r => r.MenuKey);
    }

    public IAnalyzerRunner? GetRunner(string choice) =>
        _runners.TryGetValue(choice, out var runner) ? runner : null;

    public IEnumerable<IAnalyzerRunner> GetAll() => _runners.Values;
}
