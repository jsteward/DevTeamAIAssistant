namespace DevTeamAIAssistant.Features;

public interface IAnalyzerFactory
{
    IAnalyzerRunner? GetRunner(string choice);
    IEnumerable<IAnalyzerRunner> GetAll();
}
