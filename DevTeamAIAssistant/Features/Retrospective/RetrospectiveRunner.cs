using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public class RetrospectiveRunner : AnalyzerRunnerBase<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse>
{
    public override string MenuKey => "1";
    public override string MenuLabel => "Analyze Sprint Retrospective";
    protected override string Title => "Sprint Retrospective Analyzer";
    protected override string InputPrompt => "Enter retrospective notes (type 'END' on a new line when done):";
    protected override string GetProcessingMessage(int lineCount) => "Analyzing retrospective...";

    public RetrospectiveRunner(
        IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse> analyzer,
        IAnalyzerPresenter<RetrospectiveAnalyzerResponse> presenter,
        IConsoleWriter writer,
        IConsoleReader reader)
        : base(analyzer, presenter, writer, reader) { }

    protected override RetrospectiveAnalyzerRequest BuildRequest(List<string> lines) =>
        new()
        {
            Data = string.Join("\n", lines),
            Context = "This retrospective is for a 2-week sprint in a mid-sized software development team."
        };
}
