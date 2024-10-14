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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CreditCardValidator;
using Foodies.Models;
using Foodies.Migrations;
using System.Collections.Generic;
using Foodies.Controllers;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.CodeAnalysis.Operations;
using GeminiTextGenerator.Controllers;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using SerpApi;
using System.Collections;
using FirebaseAdmin.Messaging;
public class MapController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public MapController(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = "5c2a1476c0e97f202c537b7e0459338cb9792efca2e0b763809c278a810abe74";
    }

    //public async Task<IActionResult> getTime()
    //{
    //    String apiKey = "5c2a1476c0e97f202c537b7e0459338cb9792efca2e0b763809c278a810abe74";
    //    Hashtable ht = new Hashtable();
    //    ht.Add("engine", "google_maps_directions");
    //    ht.Add("start_coords", "30.01711724128, 31.18670409319749");
    //    ht.Add("end_coords", "30.058726995198423, 31.240605762125483");

    //    GoogleSearch search = new GoogleSearch(ht, apiKey);
    //    JObject data = search.GetJson();
    //    var directions = data["directions"];
    //    if (directions != null && directions.HasValues)
    //    {
    //        // Access the first direction (if multiple exist)
    //        var firstDirection = directions[0];

    //        // Now, check if the "trips" array exists in the first direction
    //        var trips = firstDirection["trips"] as JArray;

    //        if (trips != null && trips.HasValues)
    //        {
    //            var firstTrip = trips[0];

    //            // Extract travel data
    //            var travelMode = firstTrip["travel_mode"]?.ToString();
    //            var title = firstTrip["title"]?.ToString();
    //            var distance = firstTrip["distance"]?.ToString();
    //            var duration = firstTrip["duration"]?.ToString();

    //            // Return the extracted information as part of the response
    //            return Content($"Travel Mode: {travelMode}, Title: {title}, Distance: {distance}, Duration: {duration}");
    //        }
    //        else
    //        {
    //            return Content("No trips found in the first direction.");
    //        }
    //    }
    //    else
    //    {
    //        return Content("No directions found.");
    //    }
    //}
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
