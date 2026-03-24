using DevTeamAIAssistant.Models;

namespace DevTeamAIAssistant.Response;

public class TechDebtPriorityAnalyzerResponse : IAnalyzerResponse
{
    public TechDebtAnalysis Report { get; set; }

    public TechDebtPriorityAnalyzerResponse()
    {
        Report = new TechDebtAnalysis();
    }
}
