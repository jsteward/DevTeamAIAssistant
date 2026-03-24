using Moq;
using FluentAssertions;
using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Tests.Features;

[TestFixture]
public class CodeReviewRunnerTests
{
    private Mock<IAnalyzer<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse>> _mockAnalyzer;
    private Mock<IAnalyzerPresenter<CodeReviewAnalyzerResponse>> _mockPresenter;
    private CodeReviewRunner _runner;

    [SetUp]
    public void SetUp()
    {
        _mockAnalyzer = new Mock<IAnalyzer<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse>>();
        _mockPresenter = new Mock<IAnalyzerPresenter<CodeReviewAnalyzerResponse>>();
        _runner = new CodeReviewRunner(_mockAnalyzer.Object, _mockPresenter.Object);

        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<CodeReviewAnalyzerRequest>()))
            .ReturnsAsync(new CodeReviewAnalyzerResponse());
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
        Console.SetIn(new StringReader($"public class Foo {{}}\n{endToken}"));

        await _runner.RunAsync();

        _mockAnalyzer.Verify(x => x.AnalyzeAsync(It.IsAny<CodeReviewAnalyzerRequest>()), Times.Once);
    }

    [Test]
    public async Task RunAsync_PassesEnteredCode_ToAnalyzer()
    {
        Console.SetIn(new StringReader("public class Foo {}\npublic class Bar {}\nEND"));
        CodeReviewAnalyzerRequest? captured = null;
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<CodeReviewAnalyzerRequest>()))
            .Callback<CodeReviewAnalyzerRequest>(r => captured = r)
            .ReturnsAsync(new CodeReviewAnalyzerResponse());

        await _runner.RunAsync();

        captured!.Data.Should().Be("public class Foo {}\npublic class Bar {}");
    }

    [Test]
    public async Task RunAsync_CallsPresenter_WithAnalyzerResult()
    {
        var expectedResponse = new CodeReviewAnalyzerResponse();
        _mockAnalyzer
            .Setup(x => x.AnalyzeAsync(It.IsAny<CodeReviewAnalyzerRequest>()))
            .ReturnsAsync(expectedResponse);
        Console.SetIn(new StringReader("some code\nEND"));

        await _runner.RunAsync();

        _mockPresenter.Verify(x => x.Display(expectedResponse), Times.Once);
    }
}
