using System.Text.Json.Serialization;

namespace Ask.Models
{
    internal class CompletionInput
    {
        [JsonPropertyName("model")]
        public string Model { get; init; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; init; }

        [JsonPropertyName("temperature")]
        public int Temperature { get; init; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; init; }

        public CompletionInput(string prompt, string model = "text-davinci-001", int temperature = 1, int maxTokens = 100)
        {
            Model = model;
            Prompt = prompt;
            Temperature = temperature;
            MaxTokens = maxTokens;
        }
    }
}
