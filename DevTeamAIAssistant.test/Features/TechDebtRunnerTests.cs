using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class TechDebtRunnerTests
{
    private Mock<IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse>> _mockAnalyzer;
    private Mock<IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse>> _mockPresenter;
    private Mock<IConsoleWriter> _mockWriter;
    private TechDebtRunner _runner;

    [SetUp]
    public void SetUp()
    {
        _mockAnalyzer = new Mock<IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse>>();
        _mockPresenter = new Mock<IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse>>();
        _mockWriter = new Mock<IConsoleWriter>();
        _runner = new TechDebtRunner(_mockAnalyzer.Object, _mockPresenter.Object, _mockWriter.Object, new ConsoleReader());

        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<TechDebtPriorityAnalyzerRequest>()))
            .ReturnsAsync(new TechDebtPriorityAnalyzerResponse());
    }

    [TearDown]
    public void TearDown()
    {
        Console.SetIn(new StreamReader(new System.IO.MemoryStream()));
    }

    [TestCase("END")]
    [TestCase("end")]
    [TestCase("End")]
    public async Task RunAsync_TerminatesInput_WhenEndTypedInAnyCase(string endToken)
    {
        Console.SetIn(new StringReader($"Legacy auth system\n{endToken}"));

        await _runner.RunAsync();

        _mockAnalyzer.Verify(x => x.AnalyzeAsync(It.IsAny<TechDebtPriorityAnalyzerRequest>()), Times.Once);
    }

    [Test]
    public async Task RunAsync_NumbersItems_WhenPassedToAnalyzer()
    {
        Console.SetIn(new StringReader("Legacy auth system\nNo automated backups\nEND"));
        TechDebtPriorityAnalyzerRequest? captured = null;
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<TechDebtPriorityAnalyzerRequest>()))
            .Callback<TechDebtPriorityAnalyzerRequest>(r => captured = r)
            .ReturnsAsync(new TechDebtPriorityAnalyzerResponse());

        await _runner.RunAsync();

        captured!.Data.Should().Be("1. Legacy auth system\n2. No automated backups");
    }

    [Test]
    public async Task RunAsync_DoesNotCallAnalyzer_WhenNoItemsEntered()
    {
        Console.SetIn(new StringReader("END"));

        await _runner.RunAsync();

        _mockAnalyzer.Verify(x => x.AnalyzeAsync(It.IsAny<TechDebtPriorityAnalyzerRequest>()), Times.Never);
    }

    [Test]
    public async Task RunAsync_IgnoresBlankLines_WhenCollectingItems()
    {
        Console.SetIn(new StringReader("Legacy auth system\n\n   \nNo automated backups\nEND"));
        TechDebtPriorityAnalyzerRequest? captured = null;
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<TechDebtPriorityAnalyzerRequest>()))
            .Callback<TechDebtPriorityAnalyzerRequest>(r => captured = r)
            .ReturnsAsync(new TechDebtPriorityAnalyzerResponse());

        await _runner.RunAsync();

        captured!.Data.Should().Be("1. Legacy auth system\n2. No automated backups");
    }

    [Test]
    public async Task RunAsync_CallsPresenter_WithAnalyzerResult()
    {
        var expectedResponse = new TechDebtPriorityAnalyzerResponse();
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<TechDebtPriorityAnalyzerRequest>()))
            .ReturnsAsync(expectedResponse);
        Console.SetIn(new StringReader("Legacy auth system\nEND"));

        await _runner.RunAsync();

        _mockPresenter.Verify(x => x.Display(expectedResponse), Times.Once);
    }
}
