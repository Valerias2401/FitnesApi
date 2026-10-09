using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class Booking
    {
        [Key]
        public int id_booking { get; set; }

        [Required]
        [ForeignKey("User")]
        public int id_user { get; set; }

        public virtual User? User { get; set; }

        [Required]
        [ForeignKey("Court")]
        public int id_court { get; set; }

        public virtual Court? Court { get; set; }

        [Column(TypeName = "date")]
        public DateTime booking_date { get; set; }

        public TimeSpan start_time { get; set; }

        public TimeSpan end_time { get; set; }

        public decimal payment_amount { get; set; }

        public string qr_code { get; set; } = string.Empty;

        public string status { get; set; } = "active";
    }
}