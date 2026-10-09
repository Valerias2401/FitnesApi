using FitnesApi.Attributes;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsService _analyticsService;

        public AnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        // Аналитика — только Admin
        [HttpGet("Dashboard")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Dashboard()
        {
            return await _analyticsService.GetDashboard();
        }
    }
}
