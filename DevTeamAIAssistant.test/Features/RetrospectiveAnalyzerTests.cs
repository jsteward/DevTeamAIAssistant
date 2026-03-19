using NUnit.Framework;
using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class RetrospectiveAnalyzerTests
{
    private Mock<IClaudeService> _mockClaudeService;
    private RetrospectiveAnalyzer _analyzer;

    [SetUp]
    public void SetUp()
    {
        _mockClaudeService = new Mock<IClaudeService>();
        _analyzer = new RetrospectiveAnalyzer(_mockClaudeService.Object);
    }

    [Test]
    public async Task AnalyzeRetrospectiveAsync_WithValidInput_ReturnsReport()
    {
        // Arrange
        var retrospectiveNotes = "Sprint went well. Team velocity increased.";
        var expectedReport = new RetrospectiveReport
        {
            OverallSentiment = "Positive",
            KeyThemes = new() { "Velocity improvement" },
            ActionItems = new() 
            { 
                new ActionItem 
                { 
                    Description = "Continue momentum",
                    Priority = "Medium",
                    Owner = "Scrum Master",
                    EstimatedEffortDays = 0
                }
            },
            Concerns = new(),
            Wins = new() { "Velocity increased" },
            ManagerRecommendation = "Keep up the good work"
        };

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<RetrospectiveReport>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(expectedReport);

        // Act
        var result = await _analyzer.AnalyzeRetrospectiveAsync(retrospectiveNotes);

        // Assert
        result.Should().NotBeNull();
        result.OverallSentiment.Should().Be("Positive");
        result.KeyThemes.Should().Contain("Velocity improvement");
        result.ActionItems.Should().HaveCount(1);
        result.Wins.Should().Contain("Velocity increased");
    }

    [Test]
    public async Task AnalyzeRetrospectiveAsync_CallsClaudeServiceWithCorrectPrompt()
    {
        // Arrange
        var retrospectiveNotes = "Test notes";
        string? capturedPrompt = null;
        string? capturedContext = null;

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<RetrospectiveReport>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((prompt, context) =>
            {
                capturedPrompt = prompt;
                capturedContext = context;
            })
            .ReturnsAsync(new RetrospectiveReport());

        // Act
        await _analyzer.AnalyzeRetrospectiveAsync(retrospectiveNotes);

        // Assert
        _mockClaudeService.Verify(
            x => x.AnalyzeStructuredAsync<RetrospectiveReport>(
                It.IsAny<string>(), 
                It.IsAny<string>()), 
            Times.Once);
        
        capturedPrompt.Should().Contain("Agile coach");
        capturedContext.Should().Be(retrospectiveNotes);
    }

    [TestCase("")]
    public async Task AnalyzeRetrospectiveAsync_WithEmptyInput_StillCallsService(string input)
    {
        // Arrange
        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<RetrospectiveReport>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(new RetrospectiveReport());

        // Act
        var result = await _analyzer.AnalyzeRetrospectiveAsync(input);

        // Assert
        result.Should().NotBeNull();
        _mockClaudeService.Verify(
            x => x.AnalyzeStructuredAsync<RetrospectiveReport>(
                It.IsAny<string>(), 
                It.IsAny<string>()), 
            Times.Once);
    }
}