using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        // Просмотр мероприятий — для всех
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            return await _eventService.GetAll();
        }

        // Создание мероприятия — Admin или Organizer
        [HttpPost("Create")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> Create(CreateEventRequest request)
        {
            return await _eventService.Create(request);
        }

        // Обновление организации — Admin или Organizer
        [HttpPost("UpdateOrganization")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> UpdateOrganization(UpdateEventOrganizationRequest request)
        {
            return await _eventService.UpdateOrganization(request);
        }

        // Участники — Admin или Organizer
        [HttpPost("AddParticipant")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> AddParticipant(EventParticipantRequest request)
        {
            return await _eventService.AddParticipant(request);
        }

        [HttpDelete("DeleteParticipant/{id_participant}")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> DeleteParticipant(int id_participant)
        {
            return await _eventService.DeleteParticipant(id_participant);
        }

        // Просмотр участников — любой авторизованный
        [HttpGet("GetParticipants/{id_event}")]
        [AuthorizeRole]
        public async Task<ActionResult> GetParticipants(int id_event)
        {
            return await _eventService.GetParticipants(id_event);
        }

        // Игры (турнирная сетка) — Admin или Organizer
        [HttpPost("CreateGame")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> CreateGame(CreateEventGameRequest request)
        {
            return await _eventService.CreateGame(request);
        }

        // Просмотр игр — для всех
        [HttpGet("GetGames/{id_event}")]
        public async Task<ActionResult> GetGames(int id_event)
        {
            return await _eventService.GetGames(id_event);
        }

        [HttpPost("UpdateGameResult")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> UpdateGameResult(UpdateEventGameResultRequest request)
        {
            return await _eventService.UpdateGameResult(request);
        }

        [HttpDelete("DeleteGame/{id_game}")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> DeleteGame(int id_game)
        {
            return await _eventService.DeleteGame(id_game);
        }

        // Результаты — Admin или Organizer
        [HttpPost("AddResult")]
        [AuthorizeRole("Admin", "Organizer")]
        public async Task<ActionResult> AddResult(CreateEventResultRequest request)
        {
            return await _eventService.AddResult(request);
        }

        // Просмотр результатов — для всех
        [HttpGet("GetResults")]
        public async Task<ActionResult> GetResults()
        {
            return await _eventService.GetResults();
        }
    }
}
