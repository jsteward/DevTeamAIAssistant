namespace DevTeamAIAssistant.Features.IO;

public interface IConsoleWriter
{
    void WriteLine(string? value = null);
    void Write(string value);
}
