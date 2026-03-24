using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public class TechDebtRunner : IAnalyzerRunner
{
    private readonly IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse> _analyzer;
    private readonly IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse> _presenter;

    public TechDebtRunner(IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse> analyzer, IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse> presenter)
    {
        _analyzer = analyzer;
        _presenter = presenter;
    }

    public string MenuKey => "3";
    public string MenuLabel => "Prioritize Technical Debt";

    public async Task RunAsync()
    {
        Console.WriteLine("\n--- Technical Debt Prioritizer ---");
        Console.WriteLine("Enter technical debt items (one per line, type 'END' when done):\n");

        var items = ConsoleInput.ReadLines(skipEmpty: true);

        if (!items.Any())
        {
            Console.WriteLine("No items entered.");
            return;
        }

        Console.WriteLine($"\nAnalyzing {items.Count} technical debt items...\n");
        try
        {
            var request = new TechDebtPriorityAnalyzerRequest
            {
                Data = string.Join("\n", items.Select((item, i) => $"{i + 1}. {item}"))
            };
            var response = await _analyzer.AnalyzeAsync(request);
            _presenter.Display(response);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
