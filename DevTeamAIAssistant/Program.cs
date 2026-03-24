using DevTeamAIAssistant.Features;
using DevTeamAIAssistant.Features.Presenters;
using DevTeamAIAssistant.Requests;
using DevTeamAIAssistant.Response;
using DevTeamAIAssistant.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevTeamAIAssistant;

class Program
{
    static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddHttpClient("Anthropic", client =>
        {
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddSingleton<IClaudeService, ClaudeService>();
        services.AddTransient<IAnalyzer<RetrospectiveAnalyzerRequest, RetrospectiveAnalyzerResponse>, RetrospectiveAnalyzer>();
        services.AddTransient<IAnalyzer<CodeReviewAnalyzerRequest, CodeReviewAnalyzerResponse>, CodeReviewAnalyzer>();
        services.AddTransient<IAnalyzer<TechDebtPriorityAnalyzerRequest, TechDebtPriorityAnalyzerResponse>, TechDebtPriorityAnalyzer>();
        services.AddTransient<IAnalyzerPresenter<RetrospectiveAnalyzerResponse>, RetrospectivePresenter>();
        services.AddTransient<IAnalyzerPresenter<CodeReviewAnalyzerResponse>, CodeReviewPresenter>();
        services.AddTransient<IAnalyzerPresenter<TechDebtPriorityAnalyzerResponse>, TechDebtPresenter>();
        services.AddTransient<IAnalyzerRunner, RetrospectiveRunner>();
        services.AddTransient<IAnalyzerRunner, CodeReviewRunner>();
        services.AddTransient<IAnalyzerRunner, TechDebtRunner>();
        services.AddSingleton<IAnalyzerFactory, AnalyzerFactory>();

        var provider = services.BuildServiceProvider();

        var factory = provider.GetRequiredService<IAnalyzerFactory>();

        Console.WriteLine("╔════════════════════════════════════════════════════════╗");
        Console.WriteLine("║        DevTeam AI Assistant v1.0                       ║");
        Console.WriteLine("║        AI-Powered Development Management Tools         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════╝\n");

        while (true)
        {
            Console.WriteLine("\nSelect a feature:");
            foreach (var r in factory.GetAll())
                Console.WriteLine($"{r.MenuKey}. {r.MenuLabel}");
            Console.WriteLine("X. Exit");
            Console.Write("\nYour choice: ");

            var choice = Console.ReadLine();

            if (choice?.ToUpper() == "X")
            {
                Console.WriteLine("\nGoodbye!");
                return;
            }

            var runner = choice != null ? factory.GetRunner(choice) : null;
            if (runner != null)
                await runner.RunAsync();
            else
                Console.WriteLine("Invalid choice. Please try again.");
        }
    }

}