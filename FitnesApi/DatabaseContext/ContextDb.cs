using Microsoft.EntityFrameworkCore;
using FitnesApi.Models;

namespace FitnesApi.DatabaseContext
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Abonement> Abonements { get; set; }
        public DbSet<UserAbonement> UserAbonements { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<SportSection> Sections { get; set; }
        public DbSet<SectionEnrollment> SectionEnrollments { get; set; }
        public DbSet<SectionAttendance> SectionAttendances { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<SportEvent> SportEvents { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<EventResult> EventResults { get; set; }
        public DbSet<EventGame> EventGames { get; set; }
    }
}