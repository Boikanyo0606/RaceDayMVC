using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var loginData = new { email, password };
            var response = await _apiService.PostAsync("/api/Auth/login", loginData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);

                var token = doc.RootElement.GetProperty("token").GetString();
                var role = doc.RootElement.GetProperty("role").GetString();

                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("Role", role);
                HttpContext.Session.SetString("Email", email);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password, string role)
        {
            var registerData = new { email, password, role };
            var response = await _apiService.PostAsync("/api/Auth/register", registerData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Registration failed. Email might be taken.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}