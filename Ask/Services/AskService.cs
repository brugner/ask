using Ask.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Ask.Services
{
    public class AskService
    {
        private readonly string _apiUrl = default!;
        private readonly string _apiKey = default!;
        private readonly HttpClient _httpClient;

        public AskService(string apiUrl, string apiKey, HttpClient? httpClient = null)
        {
            _apiUrl = apiUrl;
            _apiKey = apiKey;

            _httpClient = httpClient ?? new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("authorization", $"Bearer {_apiKey}");
        }

        public async Task<Answer> AskAsync(string prompt)
        {
            try
            {
                if (string.IsNullOrEmpty(_apiUrl))
                {
                    return Answer.Error("API url is missing");
                }

                if (string.IsNullOrEmpty(_apiKey))
                {
                    return Answer.Error("API key is missing");
                }

                if (string.IsNullOrEmpty(prompt))
                {
                    return Answer.Error("Please enter a prompt");
                }

                var input = new CompletionInput(prompt);
                var response = await _httpClient.PostAsync(_apiUrl, new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                var output = JsonSerializer.Deserialize<CompletionOutput>(responseString);

                return Answer.Ok(output!.Text);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return Answer.Error($"Something went wrong: the URL is incorrect");
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Answer.Error($"Something went wrong: the API key is invalid or it doesn't have permission to do this");
            }
            catch (JsonException)
            {
                return Answer.Error($"Something went wrong: the JSON response is invalid");
            }
            catch (Exception ex)
            {
                return Answer.Error($"Something went wrong: {ex.Message}");
            }
        }
    }
}
