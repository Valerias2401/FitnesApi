using FitnesApi.DatabaseContext;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FitnesApi.Services
{
    public class UserAbonementService
    {
        private readonly ContextDb _context;
        public UserAbonementService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> Buy(BuyAbonementRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.id_user == request.id_user);
            if (user == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Пользователь не найден"
                });
            }
            var abonement = await _context.Abonements
                .FirstOrDefaultAsync(x => x.id_abonement == request.id_abonement);
            if (abonement == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Абонемент не найден"
                });
            }
            if (user.balance < abonement.price)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Недостаточно средств"
                });
            }
            user.balance -= abonement.price;
            var userAbonement = new UserAbonement()
            {
                id_user = user.id_user,
                id_abonement = abonement.id_abonement,
                start_date = DateTime.Now,
                end_date = DateTime.Now.AddDays(abonement.duration_days)
            };
            await _context.UserAbonements.AddAsync(userAbonement);
            await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                status = true,
                message = "Абонемент успешно куплен"
            });
        }

        public async Task<ActionResult> GetByUser(int id_user)
        {
            var abonements = await _context.UserAbonements
                .Include(x => x.Abonement)
                .Where(x => x.id_user == id_user)
                .Select(x => new
                {
                    x.id_user_abonement,
                    x.id_abonement,
                    name = x.Abonement != null ? x.Abonement.name_abonement : "",
                    x.start_date,
                    x.end_date,
                    is_active = x.end_date.Date >= DateTime.Today
                })
                .ToListAsync();

            return new JsonResult(new { status = true, data = abonements });
        }
    }
}