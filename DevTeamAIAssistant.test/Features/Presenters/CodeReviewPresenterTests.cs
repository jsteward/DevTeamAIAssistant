using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features.CodeReview;
using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features.Presenters;

[TestFixture]
public class CodeReviewPresenterTests
{
    private Mock<IConsoleWriter> _mockWriter;
    private CodeReviewPresenter _presenter;
    private List<string> _output;

    [SetUp]
    public void SetUp()
    {
        _mockWriter = new Mock<IConsoleWriter>();
        _output = new List<string>();
        _mockWriter.Setup(w => w.WriteLine(It.IsAny<string>())).Callback<string?>(s => _output.Add(s ?? ""));
        _mockWriter.Setup(w => w.WriteLine()).Callback(() => _output.Add(""));
        _presenter = new CodeReviewPresenter(_mockWriter.Object);
    }

    private string FullOutput => string.Join("\n", _output);

    [Test]
    public void Display_WritesHeader()
    {
        _presenter.Display(new CodeReviewAnalyzerResponse());

        FullOutput.Should().Contain("CODE REVIEW ANALYSIS");
    }

    [Test]
    public void Display_WritesQualityScore()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.QualityScore = 8;

        _presenter.Display(response);

        FullOutput.Should().Contain("8/10");
    }

    [Test]
    public void Display_WritesOverallAssessment()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.OverallAssessment = "Solid implementation with minor issues";

        _presenter.Display(response);

        FullOutput.Should().Contain("Solid implementation with minor issues");
    }

    [Test]
    public void Display_WritesSecurityConcerns_WhenPresent()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.SecurityConcerns = ["SQL injection risk on line 42"];

        _presenter.Display(response);

        FullOutput.Should().Contain("Security Concerns");
        FullOutput.Should().Contain("SQL injection risk on line 42");
    }

    [Test]
    public void Display_OmitsSecurityConcernsSection_WhenEmpty()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.SecurityConcerns = [];

        _presenter.Display(response);

        FullOutput.Should().NotContain("Security Concerns");
    }

    [Test]
    public void Display_WritesBestPractices_WhenPresent()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.BestPractices = ["Uses dependency injection"];

        _presenter.Display(response);

        FullOutput.Should().Contain("Best Practices");
        FullOutput.Should().Contain("Uses dependency injection");
    }

    [Test]
    public void Display_OmitsBestPracticesSection_WhenEmpty()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.BestPractices = [];

        _presenter.Display(response);

        FullOutput.Should().NotContain("Best Practices");
    }

    [Test]
    public void Display_WritesCommentCategoryAndIssue()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.Comments =
        [
            new ReviewComment { Category = "Performance", Severity = "medium", Issue = "N+1 query detected", Suggestion = "Use eager loading" }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain("Performance");
        FullOutput.Should().Contain("N+1 query detected");
        FullOutput.Should().Contain("Use eager loading");
    }

    [Test]
    public void Display_WritesCommentsSeverityGroupLabel_InUpperCase()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.Comments =
        [
            new ReviewComment { Severity = "critical", Issue = "Auth bypass", Suggestion = "Fix immediately" }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain("[CRITICAL]");
    }

    [Test]
    public void Display_WritesLineNumber_WhenGreaterThanZero()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.Comments =
        [
            new ReviewComment { Severity = "low", Issue = "Unused variable", Suggestion = "Remove it", LineNumber = 17 }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain("Line 17");
    }

    [Test]
    public void Display_OmitsLineNumber_WhenZero()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.Comments =
        [
            new ReviewComment { Severity = "low", Issue = "Unused variable", Suggestion = "Remove it", LineNumber = 0 }
        ];

        _presenter.Display(response);

        FullOutput.Should().NotContain("Line 0");
    }

    [Test]
    public void Display_GroupsCommentsBySeverity_OrderedCriticalFirst()
    {
        var response = new CodeReviewAnalyzerResponse();
        response.Review.Comments =
        [
            new ReviewComment { Severity = "low",      Issue = "Low issue",      Suggestion = "" },
            new ReviewComment { Severity = "critical",  Issue = "Critical issue", Suggestion = "" },
            new ReviewComment { Severity = "high",      Issue = "High issue",     Suggestion = "" },
        ];

        _presenter.Display(response);

        var criticalIndex = FullOutput.IndexOf("[CRITICAL]");
        var highIndex     = FullOutput.IndexOf("[HIGH]");
        var lowIndex      = FullOutput.IndexOf("[LOW]");

        criticalIndex.Should().BeLessThan(highIndex);
        highIndex.Should().BeLessThan(lowIndex);
    }

    [Test]
    public void Display_WithNoComments_DoesNotThrow()
    {
        var act = () => _presenter.Display(new CodeReviewAnalyzerResponse());

        act.Should().NotThrow();
    }
}
