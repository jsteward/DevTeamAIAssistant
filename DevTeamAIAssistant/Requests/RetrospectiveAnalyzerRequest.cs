using System;

namespace DevTeamAIAssistant.Requests;

public class RetrospectiveAnalyzerRequest : IAnalyzerRequest
{
    public string Data {  get; set; }     
    public string Context {  get; set; }
}
