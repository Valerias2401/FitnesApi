using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }

        // Все бронирования — только Admin
        [HttpGet("GetAll")]
        [AuthorizeRole("Admin")]
        public async Task<ActionResult> GetAll()
        {
            return await _bookingService.GetAll();
        }

        // Бронирования пользователя — любой авторизованный
        [HttpGet("GetByUser/{id_user}")]
        [AuthorizeRole]
        public async Task<ActionResult> GetByUser(int id_user)
        {
            return await _bookingService.GetByUser(id_user);
        }

        // Создать бронирование — Client, Admin
        [HttpPost("Create")]
        [AuthorizeRole("Client", "Admin")]
        public async Task<ActionResult> Create(CreateBookingRequest request)
        {
            return await _bookingService.Create(request);
        }

        // Отменить бронирование — любой авторизованный
        [HttpPost("Cancel/{id_booking}")]
        [AuthorizeRole]
        public async Task<ActionResult> Cancel(int id_booking)
        {
            return await _bookingService.Cancel(id_booking);
        }
    }
}
