using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Services;
using Microsoft.Extensions.Configuration;

namespace DevTeamAIAssistant;

class Program
{
    static async Task Main(string[] args)
    {
        // Setup configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Initialize services
        var claudeService = new ClaudeService(configuration);
        var retroAnalyzer = new RetrospectiveAnalyzer(claudeService);
        var codeReviewer = new CodeReviewAnalyzer(claudeService);
        var techDebtPrioritizer = new TechDebtPriorityAnalyzer(claudeService);  // ADD THIS LINE

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║        DevTeam AI Assistant v1.0                       ║");
        Console.WriteLine("║        AI-Powered Development Management Tools         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        while (true)
        {
            Console.WriteLine("\nSelect a feature:");
            Console.WriteLine("1. Analyze Sprint Retrospective");
            Console.WriteLine("2. Review Code");
            Console.WriteLine("3. Prioritize Technical Debt");  // ADD THIS LINE
            Console.WriteLine("4. Exit");                        // CHANGE FROM 3 TO 4
            Console.Write("\nYour choice: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await RunRetrospectiveAnalysis(retroAnalyzer);
                    break;
                case "2":
                    await RunCodeReview(codeReviewer);
                    break;
                case "3":                                        // ADD THIS CASE
                    await RunTechDebtPrioritization(techDebtPrioritizer);
                    break;
                case "4":                                        // CHANGE FROM 3 TO 4
                    Console.WriteLine("\nGoodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static async Task RunRetrospectiveAnalysis(RetrospectiveAnalyzer analyzer)
    {
        Console.WriteLine("\n--- Sprint Retrospective Analyzer ---");
        Console.WriteLine("Enter retrospective notes (type 'END' on a new line when done):\n");

        var notes = new List<string>();
        string? line;
        while ((line = Console.ReadLine()) != "END")
        {
            if (line != null) notes.Add(line);
        }

        var retrospectiveText = string.Join("\n", notes);
        
        Console.WriteLine("\n🤖 Analyzing retrospective...\n");
        var request = new RetrospectiveAnalyzerRequest
        {
            Data = retrospectiveText,
            Context = "This retrospective is for a 2-week sprint in a mid-sized software development team."
        };
        try
        {
            var response = await analyzer.AnalyzeAsync(request);
            response.Display();
            //analyzer.DisplayReport(response.Report);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }

    static async Task RunCodeReview(CodeReviewAnalyzer reviewer)
    {
        Console.WriteLine("\n--- Code Review Assistant ---");
        Console.WriteLine("Enter code to review (type 'END' on a new line when done):\n");

        var codeLines = new List<string>();
        string? line;
        while ((line= Console.ReadLine()) != "END")
        {            
            if (line != null) codeLines.Add(line);
        }

var request = new CodeReviewAnalyzerRequest
        { 
            Data = string.Join("\n", codeLines),
            Context = "This code is part of a web API project using ASP.NET Core."
        };
        //var code = string.Join("\n", codeLines);
        
        Console.WriteLine("\n🤖 Reviewing code...\n");
        
        try
        {
            var result = await reviewer.AnalyzeAsync(request);
            //var review = await reviewer.ReviewCodeAsync(code);
            result.Display();
            //reviewer.DisplayReview(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }

    // ADD THIS ENTIRE METHOD
    static async Task RunTechDebtPrioritization(TechDebtPriorityAnalyzer prioritizer)
    {
        Console.WriteLine("\n--- Technical Debt Prioritizer ---");
        Console.WriteLine("Enter technical debt items (one per line, type 'END' when done):\n");

        var items = new List<string>();
        string? line;
        while ((line = Console.ReadLine()) != "END")
        {
            if (!string.IsNullOrWhiteSpace(line)) 
                items.Add(line);
        }

        if (!items.Any())
        {
            Console.WriteLine("⚠️  No items entered.");
            return;
        }

        Console.WriteLine($"\n🤖 Analyzing {items.Count} technical debt items...\n");
        
        try
        {
            var analysis = await prioritizer.PrioritizeDebtAsync(items);
            prioritizer.DisplayAnalysis(analysis);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }
}