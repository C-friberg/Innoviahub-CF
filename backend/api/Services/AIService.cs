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

        public async Task<BookingIntentDto> AskAsync(string question)
        {
            var today = DateTime.Now.ToString("yyyy-MM-dd");
            var instructions = $"""
                    Du är bokningsassistent åt Innovia.

                    Dagens datum är {today}.
                    
                    Användaren kan boka följande resurser: 
                    - Skrivbord
                    - Mötesrum
                    - VRHeadset
                    - AIServer

                    Din uppgift är att tolka användarens bokningsönskemål.

                    Returnera endast i JSON med följande properties: 
                    - resourceType
                    - date
                    - startTime
                    - endTime

                    date ska anges i formatet yyyy-MM-dd. 
                    startTime och endTime ska anges i formatet:HH:mm:ss. 

                    Tolka relativa datum som "idag", "imorgon" och liknande
                    utifrån dagens datum.

                    Om användaren anger ett datum men ingen tid ska du inte anta 00:00.
                    Om en exakt starttid inte kan bestämmas ska startTime vara null.
                    Om en exakt sluttid inte kan bestämmas ska endTime vara null.

                    Om informationen saknas så ska motsvarande värde vara null. 
                    Hitta aldrig på information som användare inte har angett. 
                """;

            var requestBody = new
            {
                model = "gpt-4.1",
                instructions = instructions,
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

            var aiText = result.Output[0].Content[0].Text; 

            var bookingIntent = JsonSerializer.Deserialize<BookingIntentDto>(aiText, new JsonSerializerOptions{PropertyNameCaseInsensitive=true}); 

            if(bookingIntent == null)
            {
                throw new Exception("Could not parse booking intent."); 
            }

            return bookingIntent; 
        }
    }
}