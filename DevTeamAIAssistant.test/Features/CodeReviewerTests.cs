using NUnit.Framework;
using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class CodeReviewerTests
{
    private Mock<IClaudeService> _mockClaudeService;
    private CodeReviewer _reviewer;

    [SetUp]
    public void SetUp()
    {
        _mockClaudeService = new Mock<IClaudeService>();
        _reviewer = new CodeReviewer(_mockClaudeService.Object);
    }

    [Test]
    public async Task ReviewCodeAsync_WithValidCode_ReturnsReviewResult()
    {
        // Arrange
        var code = "public class Test { }";
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
        var result = await _reviewer.ReviewCodeAsync(code);

        // Assert
        result.Should().NotBeNull();
        result.QualityScore.Should().Be(8);
        result.Comments.Should().HaveCount(1);
        result.BestPractices.Should().Contain("Good class structure");
    }

    [Test]
    public async Task ReviewCodeAsync_WithOptionalContext_PassesContextToService()
    {
        // Arrange
        var code = "public class Test { }";
        var context = "This is a test class for unit testing";
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
        await _reviewer.ReviewCodeAsync(code, context);

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
        await _reviewer.ReviewCodeAsync(code);

        // Assert
        capturedPrompt.Should().Contain("senior software architect");
        capturedPrompt.Should().Contain("C#");
        capturedPrompt.Should().Contain("Security");
        capturedPrompt.Should().Contain("SOLID");
    }
}