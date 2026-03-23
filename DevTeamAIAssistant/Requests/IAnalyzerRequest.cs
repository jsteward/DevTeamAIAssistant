using System;

namespace DevTeamAIAssistant.Requests;

public interface IAnalyzerRequest
{
    string Data { get; set; }
    string Context { get; set; }
}
