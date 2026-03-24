using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class RetrospectiveRunnerTests
{
    private Mock<IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse>> _mockAnalyzer;
    private Mock<IAnalyzerPresenter<RetrospectiveAnalyzerResponse>> _mockPresenter;
    private Mock<IConsoleWriter> _mockWriter;
    private RetrospectiveRunner _runner;

    [SetUp]
    public void SetUp()
    {
        _mockAnalyzer = new Mock<IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse>>();
        _mockPresenter = new Mock<IAnalyzerPresenter<RetrospectiveAnalyzerResponse>>();
        _mockWriter = new Mock<IConsoleWriter>();
        _runner = new RetrospectiveRunner(_mockAnalyzer.Object, _mockPresenter.Object, _mockWriter.Object, new ConsoleReader());

        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<RetrospectiveAnalyzerRequest>()))
            .ReturnsAsync(new RetrospectiveAnalyzerResponse());
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
        Console.SetIn(new StringReader($"Sprint went well\n{endToken}"));

        await _runner.RunAsync();

        _mockAnalyzer.Verify(x => x.AnalyzeAsync(It.IsAny<RetrospectiveAnalyzerRequest>()), Times.Once);
    }

    [Test]
    public async Task RunAsync_PassesEnteredNotes_ToAnalyzer()
    {
        Console.SetIn(new StringReader("Team velocity increased\nNo major blockers\nEND"));
        RetrospectiveAnalyzerRequest? captured = null;
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<RetrospectiveAnalyzerRequest>()))
            .Callback<RetrospectiveAnalyzerRequest>(r => captured = r)
            .ReturnsAsync(new RetrospectiveAnalyzerResponse());

        await _runner.RunAsync();

        captured!.Data.Should().Be("Team velocity increased\nNo major blockers");
    }

    [Test]
    public async Task RunAsync_CallsPresenter_WithAnalyzerResult()
    {
        var expectedResponse = new RetrospectiveAnalyzerResponse();
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<RetrospectiveAnalyzerRequest>()))
            .ReturnsAsync(expectedResponse);
        Console.SetIn(new StringReader("some notes\nEND"));

        await _runner.RunAsync();

        _mockPresenter.Verify(x => x.Display(expectedResponse), Times.Once);
    }
}
