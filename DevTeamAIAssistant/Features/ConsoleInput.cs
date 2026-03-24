namespace DevTeamAIAssistant.Features;

public static class ConsoleInput
{
    public static List<string> ReadLines(bool skipEmpty = false)
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
