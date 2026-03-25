using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Retrospective;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features.Presenters;

[TestFixture]
public class RetrospectivePresenterTests
{
    private Mock<IConsoleWriter> _mockWriter;
    private RetrospectivePresenter _presenter;
    private List<string> _output;

    [SetUp]
    public void SetUp()
    {
        _mockWriter = new Mock<IConsoleWriter>();
        _output = new List<string>();
        _mockWriter.Setup(w => w.WriteLine(It.IsAny<string>())).Callback<string?>(s => _output.Add(s ?? ""));
        _mockWriter.Setup(w => w.WriteLine()).Callback(() => _output.Add(""));
        _presenter = new RetrospectivePresenter(_mockWriter.Object);
    }

    private string FullOutput => string.Join("\n", _output);

    [Test]
    public void Display_WritesHeader()
    {
        _presenter.Display(new RetrospectiveAnalyzerResponse());

        FullOutput.Should().Contain("SPRINT RETROSPECTIVE ANALYSIS");
    }

    [Test]
    public void Display_WritesSentimentInUpperCase()
    {
        var response = new RetrospectiveAnalyzerResponse();
        response.Report.OverallSentiment = "positive";

        _presenter.Display(response);

        FullOutput.Should().Contain("POSITIVE");
    }

    [Test]
    public void Display_WritesEachKeyTheme()
    {
        var response = new RetrospectiveAnalyzerResponse();
        response.Report.KeyThemes = ["Good collaboration", "CI pipeline issues"];

        _presenter.Display(response);

        FullOutput.Should().Contain("Good collaboration");
        FullOutput.Should().Contain("CI pipeline issues");
    }

    [Test]
    public void Display_WritesEachWin()
    {
        var response = new RetrospectiveAnalyzerResponse();
        response.Report.Wins = ["Shipped on time", "Zero regressions"];

        _presenter.Display(response);

        FullOutput.Should().Contain("Shipped on time");
        FullOutput.Should().Contain("Zero regressions");
    }

    [Test]
    public void Display_WritesEachConcern()
    {
        var response = new RetrospectiveAnalyzerResponse();
        response.Report.Concerns = ["Tech debt accumulating", "Unclear requirements"];

        _presenter.Display(response);

        FullOutput.Should().Contain("Tech debt accumulating");
        FullOutput.Should().Contain("Unclear requirements");
    }

    [Test]
    public void Display_WritesActionItemWithPriorityOwnerAndEffort()
    {
        var response = new RetrospectiveAnalyzerResponse();
        response.Report.ActionItems =
        [
            new ActionItem { Description = "Fix flaky tests", Priority = "high", Owner = "Alice", EstimatedEffortDays = 3 }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain("HIGH");
        FullOutput.Should().Contain("Fix flaky tests");
        FullOutput.Should().Contain("Alice");
        FullOutput.Should().Contain("3 days");
    }

    [Test]
    public void Display_WritesManagerRecommendation()
    {
        var response = new RetrospectiveAnalyzerResponse();
        response.Report.ManagerRecommendation = "Invest in automated testing";

        _presenter.Display(response);

        FullOutput.Should().Contain("Invest in automated testing");
    }

    [Test]
    public void Display_WithEmptyLists_DoesNotThrow()
    {
        var response = new RetrospectiveAnalyzerResponse();

        var act = () => _presenter.Display(response);

        act.Should().NotThrow();
    }
}
