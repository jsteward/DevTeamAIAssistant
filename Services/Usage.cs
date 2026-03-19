using System.Text.Json.Serialization;

namespace DevTeamAIAssistant.Services;

public partial class ClaudeService
{
    private class Usage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }
}