using GeminiTextGenerator.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Foodies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : Controller
    {
        private readonly GeminiService _geminiService;

        public GeminiController(GeminiService GeminiService)
        {
            _geminiService = GeminiService;
        }

        //[HttpGet]
        //[Route("api/gemini/generate")]
        [HttpGet("generate")]
        public async Task<string> Generate(string prompt)
        {
            var result = await _geminiService.GenerateContentAsync(prompt);
            return result;
        }
    

    public IActionResult GetNearestLocation()
        {
            string prompt = "who is the nearest in distance from " +
                            "https://maps.app.goo.gl/LjJgwoMXiFACijH99 and " +
                            "https://maps.app.goo.gl/eZNADgr6koLaiKPS9 " +
                            "https://maps.app.goo.gl/9NjqpsTpsUgiGju27 " +
                            "https://maps.app.goo.gl/16JoUExy2ocbFHTz7";

            //string links = _geminiService.GenerateText(prompt);
            return Content(" gogo");
        }

    }
}
