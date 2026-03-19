using NUnit.Framework;
using FluentAssertions;
using DevTeamAIAssistant.Models;

namespace DevTeamAIAssistant.Tests.Models;

[TestFixture]
public class ModelTests
{
    [Test]
    public void RetrospectiveReport_CanBeInstantiated()
    {
        // Act
        var report = new RetrospectiveReport();

        // Assert
        report.Should().NotBeNull();
        report.KeyThemes.Should().NotBeNull();
        report.ActionItems.Should().NotBeNull();
        report.Concerns.Should().NotBeNull();
        report.Wins.Should().NotBeNull();
    }

    [Test]
    public void ActionItem_HasExpectedProperties()
    {
        // Arrange & Act
        var actionItem = new ActionItem
        {
            Description = "Test action",
            Priority = "High",
            Owner = "Team Lead",
            EstimatedEffortDays = 5
        };

        // Assert
        actionItem.Description.Should().Be("Test action");
        actionItem.Priority.Should().Be("High");
        actionItem.Owner.Should().Be("Team Lead");
        actionItem.EstimatedEffortDays.Should().Be(5);
    }

    [Test]
    public void CodeReviewResult_CanBeInstantiated()
    {
        // Act
        var result = new CodeReviewResult();

        // Assert
        result.Should().NotBeNull();
        result.Comments.Should().NotBeNull();
        result.SecurityConcerns.Should().NotBeNull();
        result.BestPractices.Should().NotBeNull();
    }

    [Test]
    public void ReviewComment_HasExpectedProperties()
    {
        // Arrange & Act
        var comment = new ReviewComment
        {
            Category = "Security",
            Severity = "Critical",
            Issue = "SQL Injection",
            Suggestion = "Use parameterized queries",
            LineNumber = 42
        };

        // Assert
        comment.Category.Should().Be("Security");
        comment.Severity.Should().Be("Critical");
        comment.Issue.Should().Be("SQL Injection");
        comment.Suggestion.Should().Be("Use parameterized queries");
        comment.LineNumber.Should().Be(42);
    }

    [Test]
    public void TechDebtAnalysis_CanBeInstantiated()
    {
        // Act
        var analysis = new TechDebtAnalysis();

        // Assert
        analysis.Should().NotBeNull();
        analysis.PrioritizedItems.Should().NotBeNull();
    }

    [Test]
    public void PrioritizedDebtItem_HasExpectedProperties()
    {
        // Arrange & Act
        var item = new PrioritizedDebtItem
        {
            Description = "Upgrade framework",
            Priority = 8,
            Impact = "High",
            Effort = "Medium",
            RoiScore = 75,
            Reasoning = "Security patches needed",
            EstimatedDays = 7,
            Dependencies = new List<string> { "Database migration" }
        };

        // Assert
        item.Priority.Should().Be(8);
        item.RoiScore.Should().Be(75);
        item.EstimatedDays.Should().Be(7);
        item.Dependencies.Should().Contain("Database migration");
    }
}