using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;
using DevTeamAIAssistant.Services;
namespace DevTeamAIAssistant.Features;

public class RetrospectiveAnalyzer : AnalyzerBase, IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse>
{
    private const int NotesInputLimit = 3000;
    protected override int MaxInputLength => NotesInputLimit;

    private readonly IClaudeService _claudeService;

    public RetrospectiveAnalyzer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<RetrospectiveAnalyzerResponse> AnalyzeAsync(RetrospectiveAnalyzerRequest request)
    {
        var response = new RetrospectiveAnalyzerResponse();
        var data = SanitizeInput(request.Data);
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

        try
        {
            response.Report = await _claudeService.AnalyzeStructuredAsync<RetrospectiveReport>(
                prompt,
                data
            );
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            response.Report = new RetrospectiveReport
            {
                OverallSentiment = "Error during analysis",
                ManagerRecommendation = "Analysis failed. Please try again."
            };
        }
        return response;
    }

}