using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public class TechDebtRunner : AnalyzerRunnerBase<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse>
{
    public override string MenuKey => "3";
    public override string MenuLabel => "Prioritize Technical Debt";
    protected override string Title => "Technical Debt Prioritizer";
    protected override string InputPrompt => "Enter technical debt items (one per line, type 'END' when done):";
    protected override bool SkipEmptyLines => true;
    protected override string NoInputMessage => "No items entered.";
    protected override string GetProcessingMessage(int lineCount) => $"Analyzing {lineCount} technical debt items...";

    public TechDebtRunner(
        IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse> analyzer,
        IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse> presenter,
        IConsoleWriter writer,
        IConsoleReader reader)
        : base(analyzer, presenter, writer, reader) { }

    protected override TechDebtPriorityAnalyzerRequest BuildRequest(List<string> lines) =>
        new()
        {
            Data = string.Join("\n", lines.Select((item, i) => $"{i + 1}. {item}"))
        };
}
