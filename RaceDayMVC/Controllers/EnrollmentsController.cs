using Microsoft.AspNetCore.Mvc;
using RaceDayMVC.Services;
using System.Text.Json;

namespace RaceDayMVC.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApiService _apiService;

        public EnrollmentsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: /Enrollments/Enroll/5
        public async Task<IActionResult> Enroll(int id)
        {
            // In a real app, we would fetch the specific event details here
            ViewBag.EventId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(int eventId, int categoryId)
        {
            var enrollmentData = new
            {
                ParticipantID = 1, // Hardcoded for now, would get from JWT claims
                CategoryID = categoryId,
                PaymentAmount = 150.00
            };

            var response = await _apiService.PostAsync("/api/Enrollments", enrollmentData);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Enrollment failed. You might already be enrolled.";
            ViewBag.EventId = eventId;
            return View();
        }

        public async Task<IActionResult> MyResults()
        {
            // Fetch enrollments/results for the current user
            var response = await _apiService.GetAsync("/api/Enrollments");
            return View();
        }
    }
}