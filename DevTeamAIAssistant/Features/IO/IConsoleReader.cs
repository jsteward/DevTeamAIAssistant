namespace DevTeamAIAssistant.Features.IO;

public interface IConsoleReader
{
    string? ReadLine();
    List<string> ReadLines(bool skipEmpty = false);
}
