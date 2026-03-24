using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;
using DevTeamAIAssistant.Services;
namespace DevTeamAIAssistant.Features;

public class TechDebtPriorityAnalyzer : AnalyzerBase, IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse>
{
    private const int ItemsInputLimit = 3000;
    protected override int MaxInputLength => ItemsInputLimit;

    private readonly IClaudeService _claudeService;

    public TechDebtPriorityAnalyzer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<TechDebtPriorityAnalyzerResponse> AnalyzeAsync(TechDebtPriorityAnalyzerRequest request)
    {
        var data = SanitizeInput(request.Data);
        var prompt = @"You are a technical architect helping prioritize technical debt.

        Analyze these technical debt items and prioritize them based on:
        - Business impact (revenue, customer satisfaction, operational efficiency)
        - Risk (security, stability, scalability)
        - Effort required
        - Dependencies
        - ROI (Return on Investment)

        Return JSON in this exact format:
        {
        ""prioritizedItems"": [
            {
            ""description"": ""item description"",
            ""priority"": 8,
            ""impact"": ""High/Medium/Low"",
            ""effort"": ""High/Medium/Low"",
            ""roiScore"": 85,
            ""reasoning"": ""why this priority"",
            ""estimatedDays"": 5,
            ""dependencies"": [""item 1"", ""item 2""]
            }
        ],
        ""overallRecommendation"": ""strategic recommendation"",
        ""riskAssessment"": ""overall risk summary"",
        ""totalEstimatedDays"": 20
        }

        Sort by priority (highest first). Be realistic about effort estimates.";
        var response = new TechDebtPriorityAnalyzerResponse();
        try
        {
            response.Report = await _claudeService.AnalyzeStructuredAsync<TechDebtAnalysis>(
                prompt,
                data
            );
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            response.Report = new TechDebtAnalysis
            {
                OverallRecommendation = "Error during analysis",
                RiskAssessment = "Analysis failed. Please try again."
            };
        }
        return response;
    }

}