using System;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.test.Features;

public interface IAnalyzer
{
    Task<IAnalyzerResponse> AnalyzeAsync(IAnalyzerRequest request); 

}

