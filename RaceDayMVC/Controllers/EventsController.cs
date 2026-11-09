using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApiService _apiService;

        public EventsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Events
        public async Task<IActionResult> Index()
        {
            var response = await _apiService.GetAsync("/api/Events");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var events = JsonSerializer.Deserialize<List<RaceEvent>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(events);
            }
            return View(new List<RaceEvent>());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RaceEvent raceEvent, IFormFile Banner)
        {
            raceEvent.OrganiserID = 1;

            // 1. Upload image to API if a file was selected
            if (Banner != null && Banner.Length > 0)
            {
                using (var stream = Banner.OpenReadStream())
                {
                    var uploadResponse = await _apiService.UploadFileAsync("/api/Upload/image", stream, Banner.FileName);
                    if (uploadResponse.IsSuccessStatusCode)
                    {
                        raceEvent.BannerUrl = await uploadResponse.Content.ReadAsStringAsync();
                    }
                }
            }

            // 2. Create the event with the new BannerUrl
            var response = await _apiService.PostAsync("/api/Events", raceEvent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(raceEvent);
        }
    }
}