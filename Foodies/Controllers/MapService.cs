using System;
using System.Collections;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GoogleApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using SerpApi;
using Newtonsoft.Json.Linq;
using SerpApi;
using Newtonsoft.Json.Linq; // For JObject and JArray
using System.Collections.Generic; // For Dictionary
using System.Net.Http; // For HttpClient
using System.Threading.Tasks;
using Newtonsoft.Json;
using Azure.Core;
using System.Net; // For async/await

public class MapService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public MapService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = "5c2a1476c0e97f202c537b7e0459338cb9792efca2e0b763809c278a810abe74";
    }

    public async Task<JObject> GetDirectionsAsync(string url)
    {
        var response = await _httpClient.GetStringAsync(url);
        var jsonData = JsonConvert.DeserializeObject<JObject>(response);
        return jsonData;
    }
    public async Task<string> ResolveGoogleMapsLink(string shortUrl)
    {
        int maxRetries = 5;  // Maximum number of retries
        int delay = 1000;    // Initial delay in milliseconds

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                // Send a GET request to the shortened URL
                HttpResponseMessage response = await _httpClient.GetAsync(shortUrl);

                // Check if the response indicates success
                if (response.IsSuccessStatusCode)
                {
                    Uri finalUri = response.RequestMessage.RequestUri;
                    string resolvedUrl = finalUri.ToString();

                    Console.WriteLine("Resolved URL: " + resolvedUrl);
                    return resolvedUrl;
                }
                else if (response.StatusCode == (HttpStatusCode)429) // Too many requests
                {
                    Console.WriteLine("Too many requests. Waiting before retrying...");
                    await Task.Delay(delay);
                    delay *= 2; // Exponential backoff
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}. {response.ReasonPhrase}");
                    // Return the original short URL on error
                    return shortUrl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error resolving URL: " + ex.Message);
                // Return the original short URL on exception
                return shortUrl;
            }
        }

        Console.WriteLine("Max retries reached. Unable to resolve URL.");
        // Return the original short URL if max retries reached
        return shortUrl;
    }



}
