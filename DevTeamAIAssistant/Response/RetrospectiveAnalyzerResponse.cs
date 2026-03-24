using DevTeamAIAssistant.Models;

namespace DevTeamAIAssistant.Response;

public class RetrospectiveAnalyzerResponse : IAnalyzerResponse
{
    public RetrospectiveReport Report { get; set; }

    public RetrospectiveAnalyzerResponse()
    {
        Report = new RetrospectiveReport();
    }
}
