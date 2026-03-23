using System;

namespace DevTeamAIAssistant.Requests;

public class CodeReviewAnalyzerRequest : IAnalyzerRequest
{    
    public string Data {  get; set; }     
     public string Context {  get; set; }
}
