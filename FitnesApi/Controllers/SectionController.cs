using FitnesApi.Attributes;
using FitnesApi.Requests;
using FitnesApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SectionController : ControllerBase
    {
        private readonly SectionService _sectionService;

        public SectionController(SectionService sectionService)
        {
            _sectionService = sectionService;
        }

        // Просмотр секций — для всех
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            return await _sectionService.GetAll();
        }

        // Создание секции — Admin или Trainer
        [HttpPost("Create")]
        [AuthorizeRole("Admin", "Trainer")]
        public async Task<ActionResult> Create(CreateSectionRequest request)
        {
            return await _sectionService.Create(request);
        }

        // Запись в секцию — любой авторизованный
        [HttpPost("Enroll")]
        [AuthorizeRole]
        public async Task<ActionResult> Enroll(EnrollSectionRequest request)
        {
            return await _sectionService.Enroll(request);
        }

        // Список записанных — Admin или Trainer
        [HttpGet("GetEnrollments/{id_section}")]
        [AuthorizeRole("Admin", "Trainer")]
        public async Task<ActionResult> GetEnrollments(int id_section)
        {
            return await _sectionService.GetEnrollments(id_section);
        }

        // Отметить посещаемость — Admin или Trainer
        [HttpPost("MarkAttendance")]
        [AuthorizeRole("Admin", "Trainer")]
        public async Task<ActionResult> MarkAttendance(MarkAttendanceRequest request)
        {
            return await _sectionService.MarkAttendance(request);
        }

        // Посещаемость — Admin или Trainer
        [HttpGet("GetAttendance/{id_section}")]
        [AuthorizeRole("Admin", "Trainer")]
        public async Task<ActionResult> GetAttendance(int id_section)
        {
            return await _sectionService.GetAttendance(id_section);
        }

        // Удалить запись — Admin или Trainer
        [HttpDelete("DeleteEnrollment/{id_enrollment}")]
        [AuthorizeRole("Admin", "Trainer")]
        public async Task<ActionResult> DeleteEnrollment(int id_enrollment)
        {
            return await _sectionService.DeleteEnrollment(id_enrollment);
        }
    }
}
