using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace GeminiTextGenerator.Controllers
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = "AIzaSyAiAs0TXruUdXdjaL-GbScXR6LtVsHRMZM";
            _httpClient.BaseAddress = new Uri("https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent");
        }

        
        
        public async Task<string> GenerateContentAsync(string prompt)
        {
            var request = new
            {
                contents = new[]
                {
                new { role = "user", parts = new[] { new { text = prompt } } }
            },
                generationConfig = new { temperature = 0, topK = 1, topP = 1, maxOutputTokens = 2048 }
            };

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            content.Headers.Add("x-goog-api-key", _apiKey);

            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            dynamic geminiResponse = JsonConvert.DeserializeObject(responseBody);

            return geminiResponse.candidates[0].content.parts[0].text;
        }
    

    private string ExtractLinks(string text)
        {
            string link = "";
            var linkPattern = @"(http|https)://[^\s]+";

            // Use Regex to find all matches in the text
            var matches = Regex.Matches(text, linkPattern);
            foreach (Match match in matches)
            {
                link = match.Value; // Add each found link to the list
                break; // Only return the first found link
            }

            return link;
        }
    }
}
