using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            return json; 
        }
    }
}