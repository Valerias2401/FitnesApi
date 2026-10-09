using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class AbonementService
    {
        private readonly ContextDb _context;

        public AbonementService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetAll()
        {
            var abonements = await _context.Abonements.ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = abonements
            });
        }

        public async Task<ActionResult> Create(CreateAbonementRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.name_abonement))
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Введите название тарифа"
                });
            }

            if (request.price <= 0)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Цена должна быть больше 0"
                });
            }

            if (request.duration_days <= 0)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Срок действия должен быть больше 0"
                });
            }

            var existingAbonement = await _context.Abonements
                .FirstOrDefaultAsync(x => x.name_abonement == request.name_abonement);

            if (existingAbonement != null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Такой тариф уже существует"
                });
            }

            var abonement = new Abonement()
            {
                name_abonement = request.name_abonement,
                price = request.price,
                duration_days = request.duration_days,
                visit_limit = request.visit_limit,
                description = request.description
            };

            await _context.Abonements.AddAsync(abonement);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Тариф успешно создан",
                abonement
            });
        }

        public async Task<ActionResult> Update(UpdateAbonementRequest request)
        {
            var abonement = await _context.Abonements
                .FirstOrDefaultAsync(x => x.id_abonement == request.id_abonement);

            if (abonement == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Тариф не найден"
                });
            }

            abonement.name_abonement = request.name_abonement;
            abonement.price = request.price;
            abonement.duration_days = request.duration_days;
            abonement.visit_limit = request.visit_limit;
            abonement.description = request.description;

            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Тариф успешно изменён",
                abonement
            });
        }

        public async Task<ActionResult> Delete(int id)
        {
            var abonement = await _context.Abonements
                .FirstOrDefaultAsync(x => x.id_abonement == id);

            if (abonement == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Тариф не найден"
                });
            }

            _context.Abonements.Remove(abonement);
            await _context.SaveChangesAsync();

            return new JsonResult(new
            {
                status = true,
                message = "Тариф успешно удалён"
            });
        }
    }
}