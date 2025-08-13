using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace TravelBooking.Service.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "AIzaSyAqFzYqZLqr_OoNFa1juLEvSS2wImfdpVU";
        private readonly string _model = "gemini-2.0-flash";

        public GeminiService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GenerateAnswerAsync(string prompt)
        {
            prompt = PreprocessPrompt(prompt);

            var url = $"https://generativelanguage.googleapis.com/v1/models/{_model}:generateContent?key={_apiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    topK = 40,
                    topP = 0.9,
                    maxOutputTokens = 2048
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(result);

            return json.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString() ?? "Sorry, I couldn't generate a response. Please try again.";
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            var embeddingModel = "embedding-001";
            var url = $"https://generativelanguage.googleapis.com/v1/models/{embeddingModel}:embedContent?key={_apiKey}";

            var requestBody = new
            {
                content = new
                {
                    parts = new[]
                    {
                        new { text = PreprocessPrompt(text) }
                    }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Gemini Embedding API Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }

            var result = await response.Content.ReadAsStringAsync();
            using var json = JsonDocument.Parse(result);

            return json.RootElement
                .GetProperty("embedding")
                .GetProperty("values")
                .EnumerateArray()
                .Select(v => v.GetSingle())
                .ToArray();
        }

        private string PreprocessPrompt(string prompt)
        {
            prompt = Regex.Replace(prompt, @"\s+", " ").Trim();

            if (!prompt.EndsWith(".") && !prompt.EndsWith("?") && !prompt.EndsWith("!"))
            {
                prompt += ".";
            }

            return prompt;
        }
    }
}