using NUnit.Framework;
using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class CodeReviewerTests
{
    private Mock<IClaudeService> _mockClaudeService;
    private CodeReviewAnalyzer _reviewer;

    [SetUp]
    public void SetUp()
    {
        _mockClaudeService = new Mock<IClaudeService>();
        _reviewer = new CodeReviewAnalyzer(_mockClaudeService.Object);
    }

    [Test]
    public async Task ReviewCodeAsync_WithValidCode_ReturnsReviewResult()
    {
        // Arrange
        var code = "public class Test { }";
        var request = new CodeReviewAnalyzerRequest
        { 
            Data = code
        };
        var expectedResult = new CodeReviewResult
        {
            OverallAssessment = "Code looks good",
            QualityScore = 8,
            Comments = new List<ReviewComment>
            {
                new ReviewComment
                {
                    Category = "Style",
                    Severity = "Low",
                    Issue = "Minor formatting issue",
                    Suggestion = "Use consistent spacing",
                    LineNumber = 1
                }
            },
            SecurityConcerns = new List<string>(),
            BestPractices = new List<string> { "Good class structure" }
        };

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<CodeReviewResult>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(expectedResult);

        // Act
        var response = await _reviewer.AnalyzeAsync(request);

        var result = response as CodeReviewAnalyzerResponse;
        // Assert
        result.Should().NotBeNull();
        result.Review.QualityScore.Should().Be(8);
        result.Review.Comments.Should().HaveCount(1);
        result.Review.BestPractices.Should().Contain("Good class structure");
    }

    [Test]
    public async Task ReviewCodeAsync_WithOptionalContext_PassesContextToService()
    {
        // Arrange
        var code = "public class Test { }";
        var context = "This is a test class for unit testing";
        string? capturedPrompt = null;

        var request = new CodeReviewAnalyzerRequest
        { 
            Data = code,
            Context = context
        };
        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<CodeReviewResult>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((prompt, ctx) =>
            {
                capturedPrompt = prompt;
            })
            .ReturnsAsync(new CodeReviewResult());

        // Act
        await _reviewer.AnalyzeAsync(request);

        // Assert
        capturedPrompt.Should().Contain(context);
        _mockClaudeService.Verify(
            x => x.AnalyzeStructuredAsync<CodeReviewResult>(
                It.IsAny<string>(), 
                It.IsAny<string>()), 
            Times.Once);
    }

    [Test]
    public async Task ReviewCodeAsync_PromptIncludesExpectedKeywords()
    {
        // Arrange
        var code = "test code";
        var request = new CodeReviewAnalyzerRequest
        { 
            Data = code
        };
        string? capturedPrompt = null;

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<CodeReviewResult>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((prompt, ctx) =>
            {
                capturedPrompt = prompt;
            })
            .ReturnsAsync(new CodeReviewResult());

        // Act
        await _reviewer.AnalyzeAsync(request);

        // Assert
        capturedPrompt.Should().Contain("senior software architect");
        capturedPrompt.Should().Contain("C#");
        capturedPrompt.Should().Contain("Security");
        capturedPrompt.Should().Contain("SOLID");
    }
}