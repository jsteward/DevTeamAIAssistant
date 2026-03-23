using NUnit.Framework;
using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Services;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class TechDebtPrioritizerTests
{
    private Mock<IClaudeService> _mockClaudeService;
    private TechDebtPriorityAnalyzer _prioritizer;

    [SetUp]
    public void SetUp()
    {
        _mockClaudeService = new Mock<IClaudeService>();
        _prioritizer = new TechDebtPriorityAnalyzer(_mockClaudeService.Object);
    }

    [Test]
    public async Task PrioritizeDebtAsync_WithValidItems_ReturnsAnalysis()
    {
        // Arrange
        var debtItems = new List<string>
        {
            "Legacy auth system",
            "No automated backups"
        };

        var expectedAnalysis = new TechDebtAnalysis
        {
            OverallRecommendation = "Fix security issues first",
            RiskAssessment = "High risk",
            TotalEstimatedDays = 15,
            PrioritizedItems = new List<PrioritizedDebtItem>
            {
                new PrioritizedDebtItem
                {
                    Description = "Legacy auth system",
                    Priority = 10,
                    Impact = "High",
                    Effort = "High",
                    RoiScore = 95,
                    Reasoning = "Critical security issue",
                    EstimatedDays = 10,
                    Dependencies = new List<string>()
                }
            }
        };

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<TechDebtAnalysis>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(expectedAnalysis);

        // Act
        var result = await _prioritizer.PrioritizeDebtAsync(debtItems);

        // Assert
        result.Should().NotBeNull();
        result.TotalEstimatedDays.Should().Be(15);
        result.PrioritizedItems.Should().HaveCount(1);
        result.PrioritizedItems[0].Priority.Should().Be(10);
    }

    [Test]
    public async Task PrioritizeDebtAsync_FormatsItemsCorrectly()
    {
        // Arrange
        var debtItems = new List<string>
        {
            "Item 1",
            "Item 2",
            "Item 3"
        };

        string? capturedContext = null;

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<TechDebtAnalysis>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .Callback<string, string>((prompt, context) =>
            {
                capturedContext = context;
            })
            .ReturnsAsync(new TechDebtAnalysis());

        // Act
        await _prioritizer.PrioritizeDebtAsync(debtItems);

        // Assert
        capturedContext.Should().Contain("1. Item 1");
        capturedContext.Should().Contain("2. Item 2");
        capturedContext.Should().Contain("3. Item 3");
    }

    [Test]
    public async Task PrioritizeDebtAsync_WithEmptyList_StillCallsService()
    {
        // Arrange
        var emptyList = new List<string>();

        _mockClaudeService
            .Setup(x => x.AnalyzeStructuredAsync<TechDebtAnalysis>(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(new TechDebtAnalysis());

        // Act
        var result = await _prioritizer.PrioritizeDebtAsync(emptyList);

        // Assert
        result.Should().NotBeNull();
        _mockClaudeService.Verify(
            x => x.AnalyzeStructuredAsync<TechDebtAnalysis>(
                It.IsAny<string>(), 
                It.IsAny<string>()), 
            Times.Once);
    }
}