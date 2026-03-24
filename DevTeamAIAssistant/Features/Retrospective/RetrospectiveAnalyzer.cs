using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;
using DevTeamAIAssistant.Services;
namespace DevTeamAIAssistant.Features;

public class RetrospectiveAnalyzer : IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse>
{
    private readonly IClaudeService _claudeService;

    public RetrospectiveAnalyzer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<RetrospectiveAnalyzerResponse> AnalyzeAsync(RetrospectiveAnalyzerRequest request)
    {
        var response = new RetrospectiveAnalyzerResponse();
        var prompt = @"You are an expert Agile coach analyzing a sprint retrospective.

        Analyze the following retrospective notes and provide a structured report in JSON format:

        {
        ""overallSentiment"": ""positive/neutral/negative"",
        ""keyThemes"": [""theme1"", ""theme2"", ""theme3""],
        ""actionItems"": [
            {
            ""description"": ""specific action"",
            ""priority"": ""high/medium/low"",
            ""owner"": ""suggested role/person"",
            ""estimatedEffortDays"": 3
            }
        ],
        ""concerns"": [""concern1"", ""concern2""],
        ""wins"": [""win1"", ""win2""],
        ""managerRecommendation"": ""specific advice for the development manager""
        }

        Focus on actionable insights and realistic recommendations.";

        var result = await _claudeService.AnalyzeStructuredAsync<RetrospectiveReport>(
            prompt, 
            request.Data
        );
        response.Report = result;
        return response;
    }

}