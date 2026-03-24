namespace DevTeamAIAssistant.Features.IO;

public class ConsoleReader : IConsoleReader
{
    public string? ReadLine() => Console.ReadLine();

    public List<string> ReadLines(bool skipEmpty = false)
    {
        var lines = new List<string>();
        string? line;
        while (!string.Equals(line = Console.ReadLine(), "END", StringComparison.OrdinalIgnoreCase))
        {
            if (skipEmpty ? !string.IsNullOrWhiteSpace(line) : line != null)
                lines.Add(line!);
        }
        return lines;
    }
}
