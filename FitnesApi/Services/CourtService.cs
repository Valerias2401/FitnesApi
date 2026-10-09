using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class CourtService
    {
        private readonly ContextDb _context;

        public CourtService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetAll()
        {
            var courts = await _context.Courts.ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = courts
            });
        }

        public async Task<ActionResult> Create(CreateCourtRequest request)
        {
            var court = new Court()
            {
                name = request.name,
                sport_type = request.sport_type,
                price_per_hour = request.price_per_hour
            };

            await _context.Courts.AddAsync(court);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Площадка успешно создана",
                court = court
            });
        }
    }
}