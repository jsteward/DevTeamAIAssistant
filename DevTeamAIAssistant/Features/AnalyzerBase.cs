using System.Text.RegularExpressions;

namespace DevTeamAIAssistant.Features;

public abstract partial class AnalyzerBase
{
    private const int RegexTimeoutMs = 1000;

    protected abstract int MaxInputLength { get; }

    protected string SanitizeInput(string input)
    {
        var sanitized = new string(input.Where(c => !char.IsControl(c)).ToArray());
        sanitized = InjectionPattern().Replace(sanitized, m => $"[REMOVED:{m.Value.Trim()}]");
        return sanitized.Length > MaxInputLength ? sanitized[..MaxInputLength] : sanitized;
    }

    // Matches phrases commonly used to hijack LLM instructions
    [GeneratedRegex(
        @"\b(ignore\s+(all\s+)?(previous|prior|above|earlier)\s+instructions?|" +
        @"disregard\s+(all\s+)?(previous|prior|above|earlier)\s+instructions?|" +
        @"forget\s+(all\s+)?(previous|prior|above|earlier)\s+instructions?|" +
        @"new\s+instructions?:|" +
        @"system\s*prompt|" +
        @"you\s+are\s+now|" +
        @"act\s+as\s+(?!a\s+senior)|" +
        @"pretend\s+(you\s+are|to\s+be)|" +
        @"override\s+(previous\s+)?instructions?|" +
        @"</?(context|system|prompt|instruction)>)",
        RegexOptions.IgnoreCase,
        matchTimeoutMilliseconds: RegexTimeoutMs)]
    private static partial Regex InjectionPattern();
}
