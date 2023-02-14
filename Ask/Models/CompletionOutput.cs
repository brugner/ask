using System.Text.Json.Serialization;

namespace Ask.Models
{
    internal class CompletionOutput
    {
        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; } = default!;

        internal string Text => Choices[0].Text;
    }

    internal class Choice
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = default!;
    }
}
