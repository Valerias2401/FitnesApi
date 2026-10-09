using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;
namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAbonementController : ControllerBase
    {
        private readonly UserAbonementService _userAbonementService;
        public UserAbonementController(UserAbonementService userAbonementService)
        {
            _userAbonementService = userAbonementService;
        }

        [HttpPost("Buy")]
        [AuthorizeRole("Client", "Admin")]
        public async Task<ActionResult> Buy(BuyAbonementRequest request)
        {
            return await _userAbonementService.Buy(request);
        }

        [HttpGet("GetByUser/{id_user}")]
        [AuthorizeRole]
        public async Task<ActionResult> GetByUser(int id_user)
        {
            return await _userAbonementService.GetByUser(id_user);
        }
    }
}