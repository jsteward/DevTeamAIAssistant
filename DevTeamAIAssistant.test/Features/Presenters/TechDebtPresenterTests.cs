using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.TechDebt;
using DevTeamAIAssistant.Models;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features.Presenters;

[TestFixture]
public class TechDebtPresenterTests
{
    private Mock<IConsoleWriter> _mockWriter;
    private TechDebtPresenter _presenter;
    private List<string> _output;

    [SetUp]
    public void SetUp()
    {
        _mockWriter = new Mock<IConsoleWriter>();
        _output = new List<string>();
        _mockWriter.Setup(w => w.WriteLine(It.IsAny<string>())).Callback<string?>(s => _output.Add(s ?? ""));
        _mockWriter.Setup(w => w.WriteLine()).Callback(() => _output.Add(""));
        _presenter = new TechDebtPresenter(_mockWriter.Object);
    }

    private string FullOutput => string.Join("\n", _output);

    [Test]
    public void Display_WritesHeader()
    {
        _presenter.Display(new TechDebtPriorityAnalyzerResponse());

        FullOutput.Should().Contain("TECHNICAL DEBT PRIORITIZATION");
    }

    [Test]
    public void Display_WritesRiskAssessment()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.RiskAssessment = "High risk of system instability";

        _presenter.Display(response);

        FullOutput.Should().Contain("High risk of system instability");
    }

    [Test]
    public void Display_WritesTotalEstimatedDays()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.TotalEstimatedDays = 42;

        _presenter.Display(response);

        FullOutput.Should().Contain("42 days");
    }

    [Test]
    public void Display_WritesStrategicRecommendation()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.OverallRecommendation = "Tackle auth debt first";

        _presenter.Display(response);

        FullOutput.Should().Contain("Tackle auth debt first");
    }

    [TestCase(8,  "[HIGH]")]
    [TestCase(10, "[HIGH]")]
    [TestCase(5,  "[MED]")]
    [TestCase(7,  "[MED]")]
    [TestCase(1,  "[LOW]")]
    [TestCase(4,  "[LOW]")]
    public void Display_ShowsCorrectPriorityIndicator(int priority, string expectedIndicator)
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.PrioritizedItems =
        [
            new PrioritizedDebtItem { Priority = priority, Description = "Some debt" }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain(expectedIndicator);
    }

    [Test]
    public void Display_WritesItemDescriptionAndReasoning()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.PrioritizedItems =
        [
            new PrioritizedDebtItem
            {
                Priority    = 6,
                Description = "Refactor authentication module",
                Reasoning   = "Reduces coupling and improves testability",
                RoiScore    = 75,
                EstimatedDays = 5
            }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain("Refactor authentication module");
        FullOutput.Should().Contain("Reduces coupling and improves testability");
        FullOutput.Should().Contain("75/100");
        FullOutput.Should().Contain("5 days");
    }

    [Test]
    public void Display_WritesDependencies_WhenPresent()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.PrioritizedItems =
        [
            new PrioritizedDebtItem
            {
                Priority     = 7,
                Description  = "Upgrade ORM",
                Dependencies = ["Database migration", "API contract update"]
            }
        ];

        _presenter.Display(response);

        FullOutput.Should().Contain("Dependencies");
        FullOutput.Should().Contain("Database migration");
        FullOutput.Should().Contain("API contract update");
    }

    [Test]
    public void Display_OmitsDependenciesLine_WhenEmpty()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.PrioritizedItems =
        [
            new PrioritizedDebtItem { Priority = 5, Description = "No deps item", Dependencies = [] }
        ];

        _presenter.Display(response);

        FullOutput.Should().NotContain("Dependencies");
    }

    [Test]
    public void Display_SortsItemsByPriorityDescending()
    {
        var response = new TechDebtPriorityAnalyzerResponse();
        response.Report.PrioritizedItems =
        [
            new PrioritizedDebtItem { Priority = 3, Description = "Low priority task" },
            new PrioritizedDebtItem { Priority = 9, Description = "High priority task" },
            new PrioritizedDebtItem { Priority = 6, Description = "Medium priority task" },
        ];

        _presenter.Display(response);

        var highIndex   = FullOutput.IndexOf("High priority task");
        var medIndex    = FullOutput.IndexOf("Medium priority task");
        var lowIndex    = FullOutput.IndexOf("Low priority task");

        highIndex.Should().BeLessThan(medIndex);
        medIndex.Should().BeLessThan(lowIndex);
    }

    [Test]
    public void Display_WithNoItems_DoesNotThrow()
    {
        var act = () => _presenter.Display(new TechDebtPriorityAnalyzerResponse());

        act.Should().NotThrow();
    }
}
