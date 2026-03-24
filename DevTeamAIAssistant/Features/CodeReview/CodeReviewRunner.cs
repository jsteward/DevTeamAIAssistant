using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;

namespace DevTeamAIAssistant.Features;

public class CodeReviewRunner : IAnalyzerRunner
{
    private readonly IAnalyzer<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse> _analyzer;
    private readonly IAnalyzerPresenter<CodeReviewAnalyzerResponse> _presenter;

    public CodeReviewRunner(IAnalyzer<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse> analyzer, IAnalyzerPresenter<CodeReviewAnalyzerResponse> presenter)
    {
        _analyzer = analyzer;
        _presenter = presenter;
    }

    public string MenuKey => "2";
    public string MenuLabel => "Review Code";

    public async Task RunAsync()
    {
        Console.WriteLine("\n--- Code Review Assistant ---");
        Console.WriteLine("Enter code to review (type 'END' on a new line when done):\n");

        var request = new CodeReviewAnalyzerRequest
        {
            Data = string.Join("\n", ConsoleInput.ReadLines()),
            Context = "This code is part of a web API project using ASP.NET Core."
        };

        Console.WriteLine("\nReviewing code...\n");
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
