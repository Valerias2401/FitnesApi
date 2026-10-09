using FitnesApi.DatabaseContext;
using FitnesApi.Hubs;
using FitnesApi.Models;
using FitnesApi.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class BookingService
    {
        private readonly ContextDb _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public BookingService(ContextDb context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<ActionResult> GetAll()
        {
            var bookings = await _context.Bookings
                .Include(x => x.User)
                .Include(x => x.Court)
                .Select(x => new
                {
                    x.id_booking,
                    x.id_user,
                    user_name = x.User != null ? x.User.full_name : "",
                    x.id_court,
                    court_name = x.Court != null ? x.Court.name : "",
                    sport_type = x.Court != null ? x.Court.sport_type : "",
                    x.booking_date,
                    x.start_time,
                    x.end_time,
                    x.payment_amount,
                    x.qr_code,
                    x.status
                })
                .OrderByDescending(x => x.booking_date)
                .ThenBy(x => x.start_time)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = bookings
            });
        }

        public async Task<ActionResult> GetByUser(int id_user)
        {
            var bookings = await _context.Bookings
                .Include(x => x.User)
                .Include(x => x.Court)
                .Where(x => x.id_user == id_user)
                .Select(x => new
                {
                    x.id_booking,
                    x.id_user,
                    user_name = x.User != null ? x.User.full_name : "",
                    x.id_court,
                    court_name = x.Court != null ? x.Court.name : "",
                    sport_type = x.Court != null ? x.Court.sport_type : "",
                    x.booking_date,
                    x.start_time,
                    x.end_time,
                    x.payment_amount,
                    x.qr_code,
                    x.status
                })
                .OrderByDescending(x => x.booking_date)
                .ThenBy(x => x.start_time)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = bookings
            });
        }

        public async Task<ActionResult> Create(CreateBookingRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.id_user == request.id_user);

            if (user == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Пользователь не найден"
                });
            }

            if (user.id_role != 3 && user.id_role != 4)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Бронировать площадки может только клиент или организатор"
                });
            }

            var court = await _context.Courts.FirstOrDefaultAsync(x => x.id_court == request.id_court);

            if (court == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Площадка не найдена"
                });
            }

            if (request.end_time <= request.start_time)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Время окончания должно быть позже времени начала"
                });
            }

            var busyBooking = await _context.Bookings.FirstOrDefaultAsync(x =>
                x.id_court == request.id_court &&
                x.booking_date.Date == request.booking_date.Date &&
                x.status == "active" &&
                request.start_time < x.end_time &&
                request.end_time > x.start_time);

            if (busyBooking != null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Площадка уже занята на выбранное время"
                });
            }

            decimal hours = (decimal)(request.end_time - request.start_time).TotalHours;
            decimal paymentAmount = court.price_per_hour * hours;

            var booking = new Booking()
            {
                id_user = request.id_user,
                id_court = request.id_court,
                booking_date = request.booking_date.Date,
                start_time = request.start_time,
                end_time = request.end_time,
                payment_amount = paymentAmount,
                qr_code = "[ здесь мог бы быть QR-код, но я не умею ]",
                status = "active"
            };

            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Создано новое бронирование площадки");

            return new JsonResult(new
            {
                status = true,
                message = "Бронирование успешно создано. QR-код сформирован",
                booking
            });
        }

        public async Task<ActionResult> Cancel(int id_booking)
        {
            var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.id_booking == id_booking);

            if (booking == null)
            {
                return new JsonResult(new
                {
                    status = false,
                    message = "Бронирование не найдено"
                });
            }

            booking.status = "cancelled";

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", "Бронирование отменено");

            return new JsonResult(new
            {
                status = true,
                message = "Бронирование отменено"
            });
        }
    }
}