using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtController : ControllerBase
    {
        private readonly CourtService _courtService;

        public CourtController(CourtService courtService)
        {
            _courtService = courtService;
        }

        // Просмотр площадок — для всех
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            return await _courtService.GetAll();
        }

        // Создание площадки — только Admin
        [HttpPost("Create")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Create(CreateCourtRequest request)
        {
            return await _courtService.Create(request);
        }
    }
}
