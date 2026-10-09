using FitnesApi.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class BookingCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BookingCleanupService> _logger;

        public BookingCleanupService(IServiceScopeFactory scopeFactory, ILogger<BookingCleanupService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoCleanup();
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task DoCleanup()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ContextDb>();

            var today = DateTime.Today;

            var expiredBookings = await context.Bookings
                .Where(x => x.status == "active" && x.booking_date.Date < today)
                .ToListAsync();

            foreach (var booking in expiredBookings)
            {
                booking.status = "completed";
            }

            var expiredAbonements = await context.UserAbonements
                .Where(x => x.end_date.Date < today)
                .ToListAsync();

            var expiredSections = await context.SportEvents
                .Where(x => x.status == "planned" && x.event_date.Date < today)
                .ToListAsync();

            foreach (var ev in expiredSections)
            {
                ev.status = "completed";
            }

            await context.SaveChangesAsync();

            _logger.LogInformation($"Cleanup выполнен: {expiredBookings.Count} бронирований, {expiredSections.Count} мероприятий завершено");
        }
    }
}