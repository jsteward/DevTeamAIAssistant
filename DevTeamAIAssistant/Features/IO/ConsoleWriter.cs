namespace DevTeamAIAssistant.Features.IO;

public class ConsoleWriter : IConsoleWriter
{
    public void WriteLine(string? value = null) => Console.WriteLine(value);
    public void Write(string value) => Console.Write(value);
}
