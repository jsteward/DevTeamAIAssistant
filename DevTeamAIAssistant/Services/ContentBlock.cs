using System.Text.Json.Serialization;

namespace DevTeamAIAssistant.Services;

public partial class ClaudeService
{
    private class ContentBlock
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }
}