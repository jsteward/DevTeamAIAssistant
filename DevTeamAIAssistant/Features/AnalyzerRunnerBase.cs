using DevTeamAIAssistant.Features.IO;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public abstract class AnalyzerRunnerBase<TRequest, TResponse> : IAnalyzerRunner
    where TRequest : IAnalyzerRequest
    where TResponse : IAnalyzerResponse
{
    private readonly IAnalyzer<TRequest, TResponse> _analyzer;
    private readonly IAnalyzerPresenter<TResponse> _presenter;
    private readonly IConsoleReader _reader;
    protected readonly IConsoleWriter Writer;

    public abstract string MenuKey { get; }
    public abstract string MenuLabel { get; }
    protected abstract string Title { get; }
    protected abstract string InputPrompt { get; }
    protected abstract TRequest BuildRequest(List<string> lines);

    protected virtual bool SkipEmptyLines => false;
    protected virtual string NoInputMessage => "No input provided.";
    protected virtual string GetProcessingMessage(int lineCount) => "Processing...";

    protected AnalyzerRunnerBase(
        IAnalyzer<TRequest, TResponse> analyzer,
        IAnalyzerPresenter<TResponse> presenter,
        IConsoleWriter writer,
        IConsoleReader reader)
    {
        _analyzer = analyzer;
        _presenter = presenter;
        Writer = writer;
        _reader = reader;
    }

    public async Task RunAsync()
    {
        Writer.WriteLine($"\n--- {Title} ---");
        Writer.WriteLine(InputPrompt);

        var lines = _reader.ReadLines(SkipEmptyLines);

        if (!lines.Any())
        {
            Writer.WriteLine(NoInputMessage);
            return;
        }

        Writer.WriteLine($"\n{GetProcessingMessage(lines.Count)}\n");
        try
        {
            var response = await _analyzer.AnalyzeAsync(BuildRequest(lines));
            _presenter.Display(response);
        }
        catch (Exception ex)
        {
            Writer.WriteLine($"Error: {ex.Message}");
        }
    }
}
