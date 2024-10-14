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
using Newtonsoft.Json; // For async/await

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
        try
        {
            // Send a GET request to the shortened URL
            HttpResponseMessage response = await _httpClient.GetAsync(shortUrl);

            response.EnsureSuccessStatusCode();

            Uri finalUri = response.RequestMessage.RequestUri;

            string resolvedUrl = finalUri.ToString();

            Console.WriteLine("Resolved URL: " + resolvedUrl);

            return resolvedUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error resolving URL: " + ex.Message);
            return null;
        }
    }

 
}
