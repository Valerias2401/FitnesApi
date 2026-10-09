using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        // Публичные — без токена
        [HttpPost("Register")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            return await _userService.Register(request);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            return await _userService.Login(request);
        }
    }
}
