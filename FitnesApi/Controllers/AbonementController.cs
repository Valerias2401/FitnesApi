using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AbonementController : ControllerBase
    {
        private readonly AbonementService _abonementService;

        public AbonementController(AbonementService abonementService)
        {
            _abonementService = abonementService;
        }

        // Просмотр абонементов — для всех
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            return await _abonementService.GetAll();
        }

        // Управление абонементами — только Admin
        [HttpPost("Create")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Create(CreateAbonementRequest request)
        {
            return await _abonementService.Create(request);
        }

        [HttpPut("Update")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Update(UpdateAbonementRequest request)
        {
            return await _abonementService.Update(request);
        }

        [HttpDelete("Delete/{id}")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            return await _abonementService.Delete(id);
        }
    }
}
