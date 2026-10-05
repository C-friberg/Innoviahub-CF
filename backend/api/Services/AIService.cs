using System.Text.Json;
using api.Dtos.AIDtos;
using System.Net.Http.Json;

namespace api.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        public AIService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("openai");
        }

        public async Task<string> AskAsync(string question)
        {
            var requestBody = new
            {
                model = "gpt-4.1",
                input = question
            };

            var response = await _httpClient.PostAsJsonAsync("responses", requestBody);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<OpenAIResponseDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null || result.Output.Count == 0)
            {
                throw new Exception("OpenAI returned no output.");
            }

            if (result.Output[0].Content.Count == 0)
            {
                throw new Exception("OpenAi returned no content.");
            }

            return result.Output[0].Content[0].Text;
        }
    }
}