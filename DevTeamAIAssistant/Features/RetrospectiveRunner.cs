using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public class RetrospectiveRunner : IAnalyzerRunner
{
    private readonly IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse> _analyzer;
    private readonly IAnalyzerPresenter<RetrospectiveAnalyzerResponse> _presenter;

    public RetrospectiveRunner(IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse> analyzer, IAnalyzerPresenter<RetrospectiveAnalyzerResponse> presenter)
    {
        _analyzer = analyzer;
        _presenter = presenter;
    }

    public string MenuKey => "1";
    public string MenuLabel => "Analyze Sprint Retrospective";

    public async Task RunAsync()
    {
        Console.WriteLine("\n--- Sprint Retrospective Analyzer ---");
        Console.WriteLine("Enter retrospective notes (type 'END' on a new line when done):\n");

        var notes = new List<string>();
        string? line;
        while (!string.Equals(line = Console.ReadLine(), "END", StringComparison.OrdinalIgnoreCase))
        {
            if (line != null) notes.Add(line);
        }

        var request = new RetrospectiveAnalyzerRequest
        {
            Data = string.Join("\n", notes),
            Context = "This retrospective is for a 2-week sprint in a mid-sized software development team."
        };

        Console.WriteLine("\nAnalyzing retrospective...\n");
        try
        {
            var response = await _analyzer.AnalyzeAsync(request);
            _presenter.Display(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
