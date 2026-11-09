using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Models;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

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

        public IActionResult Privacy() => View();
    }
}
