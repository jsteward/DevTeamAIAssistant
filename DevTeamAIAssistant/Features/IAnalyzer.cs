using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public interface IAnalyzer<TRequest, TResponse>
    where TRequest : IAnalyzerRequest
    where TResponse : IAnalyzerResponse
{
    Task<TResponse> AnalyzeAsync(TRequest request);
}
