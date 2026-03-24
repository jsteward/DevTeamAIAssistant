using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public class CodeReviewRunner : AnalyzerRunnerBase<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse>
{
    public override string MenuKey => "2";
    public override string MenuLabel => "Review Code";
    protected override string Title => "Code Review Assistant";
    protected override string InputPrompt => "Enter code to review (type 'END' on a new line when done):";
    protected override string GetProcessingMessage(int lineCount) => "Reviewing code...";

    public CodeReviewRunner(
        IAnalyzer<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse> analyzer,
        IAnalyzerPresenter<CodeReviewAnalyzerResponse> presenter,
        IConsoleWriter writer,
        IConsoleReader reader)
        : base(analyzer, presenter, writer, reader) { }

    protected override CodeReviewAnalyzerRequest BuildRequest(List<string> lines) =>
        new()
        {
            Data = string.Join("\n", lines),
            Context = "This code is part of a web API project using ASP.NET Core."
        };
}
