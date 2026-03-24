using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;
using DevTeamAIAssistant.Services;
namespace DevTeamAIAssistant.Features;

public class TechDebtPriorityAnalyzer : IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse>
{
    private readonly IClaudeService _claudeService;

    public TechDebtPriorityAnalyzer(IClaudeService claudeService)
    {
        _claudeService = claudeService;
    }

    public async Task<TechDebtPriorityAnalyzerResponse> AnalyzeAsync(TechDebtPriorityAnalyzerRequest request)
    {      
        
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
        response.Report = await _claudeService.AnalyzeStructuredAsync<TechDebtAnalysis>(
            prompt, request.Data
            
        );
        return response;
    }

}