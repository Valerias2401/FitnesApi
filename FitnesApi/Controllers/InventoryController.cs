using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService _inventoryService;

        public InventoryController(InventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        // Инвентарь — только Admin
        [HttpGet("GetAll")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> GetAll()
        {
            return await _inventoryService.GetAll();
        }

        [HttpPost("Create")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Create(CreateInventoryRequest request)
        {
            return await _inventoryService.Create(request);
        }

        [HttpPost("UpdateStatus")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> UpdateStatus(UpdateInventoryStatusRequest request)
        {
            return await _inventoryService.UpdateStatus(request);
        }

        [HttpDelete("Delete/{id_inventory}")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> Delete(int id_inventory)
        {
            return await _inventoryService.Delete(id_inventory);
        }
    }
}
