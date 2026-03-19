using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DevTeamAIAssistant.Services;

public class ClaudeService : IClaudeService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;
    private const string AnthropicApiUrl = "https://api.anthropic.com/v1/messages";

    public ClaudeService(IConfiguration configuration)
    {
        _apiKey = configuration["Anthropic:ApiKey"] 
            ?? throw new InvalidOperationException("Anthropic API key not configured");
        _model = configuration["Anthropic:Model"] ?? "claude-sonnet-4-20250514";
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);
        _httpClient.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<string> AnalyzeAsync(string prompt, string context)
    {
        var requestBody = new
        {
            model = _model,
            max_tokens = 4096,
            messages = new[]
            {
                new
                {
                    role = "user",
                    content = $"{prompt}\n\nContext:\n{context}"
                }
            }
        };

        var jsonContent = JsonSerializer.Serialize(requestBody);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync(AnthropicApiUrl, httpContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"API Error: {response.StatusCode}");
                Console.WriteLine($"Response: {responseBody}");
                throw new HttpRequestException($"Claude API returned {response.StatusCode}");
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var apiResponse = JsonSerializer.Deserialize<ClaudeApiResponse>(responseBody, options);

            // Extract text from the content blocks
            var textContent = apiResponse?.Content?
                .Where(c => c.Type == "text")
                .Select(c => c.Text)
                .FirstOrDefault();

            return textContent 
                ?? throw new InvalidOperationException("No text content in response");
        }
        catch (HttpRequestException ex)
        {
            throw new InvalidOperationException($"Failed to call Claude API: {ex.Message}", ex);
        }
    }

    public async Task<T> AnalyzeStructuredAsync<T>(string prompt, string context) 
        where T : class
    {
        var jsonPrompt = $@"{prompt}

IMPORTANT: Respond ONLY with valid JSON. No markdown, no explanation, just the JSON object.

Context:
{context}";

        var response = await AnalyzeAsync(jsonPrompt, "");
        
        // Clean potential markdown formatting
        var cleanedResponse = response
            .Replace("```json", "")
            .Replace("```", "")
            .Trim();
        
        try
        {
            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            };
            
            return JsonSerializer.Deserialize<T>(cleanedResponse, options) 
                ?? throw new InvalidOperationException("Failed to parse AI response");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"\n❌ JSON Parsing Error:");
            Console.WriteLine($"Raw response:\n{cleanedResponse}\n");
            Console.WriteLine($"Error: {ex.Message}\n");
            throw new InvalidOperationException("Claude returned invalid JSON format", ex);
        }
    }

    // Response models for API deserialization
    private class ClaudeApiResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public List<ContentBlock>? Content { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("stop_reason")]
        public string? StopReason { get; set; }

        [JsonPropertyName("stop_sequence")]
        public string? StopSequence { get; set; }

        [JsonPropertyName("usage")]
        public Usage? Usage { get; set; }
    }

    private class ContentBlock
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("text")]
        public string? Text { get; set; }
    }

    private class Usage
    {
        [JsonPropertyName("input_tokens")]
        public int InputTokens { get; set; }

        [JsonPropertyName("output_tokens")]
        public int OutputTokens { get; set; }
    }
}