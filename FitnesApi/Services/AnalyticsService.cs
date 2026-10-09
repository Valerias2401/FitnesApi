using FitnesApi.DatabaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnesApi.Services
{
    public class AnalyticsService
    {
        private readonly ContextDb _context;

        public AnalyticsService(ContextDb context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetDashboard()
        {
            var usersCount = await _context.Users.CountAsync();

            var totalBookings = await _context.Bookings.CountAsync();

            var activeBookings = await _context.Bookings
                .CountAsync(x => x.status == "active");

            var cancelledBookings = await _context.Bookings
                .CountAsync(x => x.status == "cancelled");

            var totalRevenue = await _context.Bookings
                .Where(x => x.status == "active")
                .SumAsync(x => x.payment_amount);

            var courtsCount = await _context.Courts.CountAsync();
            var sectionsCount = await _context.Sections.CountAsync();
            var sectionEnrollmentsCount = await _context.SectionEnrollments.CountAsync();

            var attendanceTotal = await _context.SectionAttendances.CountAsync();
            var attendancePresent = await _context.SectionAttendances.CountAsync(x => x.is_present);

            var eventsCount = await _context.SportEvents.CountAsync();
            var eventParticipantsCount = await _context.EventParticipants.CountAsync();

            var inventoryCount = await _context.InventoryItems.CountAsync();
            var repairCount = await _context.InventoryItems.CountAsync(x => x.need_repair);
            var purchaseRequiredCount = await _context.InventoryItems.CountAsync(x => x.purchase_required);

            var popularCourts = await _context.Bookings
                .Include(x => x.Court)
                .GroupBy(x => new
                {
                    x.id_court,
                    court_name = x.Court != null ? x.Court.name : ""
                })
                .Select(x => new
                {
                    x.Key.id_court,
                    x.Key.court_name,
                    bookings = x.Count(),
                    bookings_count = x.Count()
                })
                .OrderByDescending(x => x.bookings)
                .ToListAsync();

            var popularSections = await _context.SectionEnrollments
                .Include(x => x.SportSection)
                .GroupBy(x => new
                {
                    x.id_section,
                    section_name = x.SportSection != null ? x.SportSection.name : ""
                })
                .Select(x => new
                {
                    x.Key.id_section,
                    x.Key.section_name,
                    participants = x.Count(),
                    enrollments_count = x.Count()
                })
                .OrderByDescending(x => x.participants)
                .ToListAsync();

            return new JsonResult(new
            {
                status = true,
                data = new
                {
                    usersCount,
                    totalBookings,
                    bookingsCount = totalBookings,
                    activeBookings,
                    cancelledBookings,
                    totalRevenue,
                    courtsCount,
                    sectionsCount,
                    sectionEnrollmentsCount,
                    attendanceTotal,
                    attendancePresent,
                    attendanceAbsent = attendanceTotal - attendancePresent,
                    eventsCount,
                    eventParticipantsCount,
                    inventoryCount,
                    repairCount,
                    purchaseRequiredCount,
                    popularCourts,
                    popularSections
                }
            });
        }
    }
}