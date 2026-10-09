using System.ComponentModel.DataAnnotations;

namespace FitnesApi.Models
{
    public class Court
    {
        [Key]
        public int id_court { get; set; }

        [Required]
        public string name { get; set; } = string.Empty;

        public string? sport_type { get; set; }

        [Required]
        public decimal price_per_hour { get; set; }
    }
}