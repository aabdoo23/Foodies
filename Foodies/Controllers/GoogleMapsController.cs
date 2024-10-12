using System.Diagnostics;
using Foodies.Models;
using Foodies.ViewModels;
using Foodies.ViewModels.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Identity.Client;

namespace Foodies.Controllers
{
    public class GoogleMapsController : Controller
    {
        private readonly HttpClient _httpClient;



        public GoogleMapsController()
            {
            _httpClient = new HttpClient();
        }

        public IActionResult location()
        {
            return View();
        }
        public async Task<IActionResult> distance()
        {
            string origin = "Seattle";
            string destination = "San Francisco";
            string apiKey = "AIzaSyC4zv8SAzsKS3GzcSreWRg73Bt_NOpuRYw"; // Replace with your actual Google Maps API key

            string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destination}&key={apiKey}";

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            else
            {
                return Content("Error fetching data from Google API");
            }
        }

    }
}
