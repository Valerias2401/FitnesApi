using System.ComponentModel.DataAnnotations;

namespace FitnesApi.Models
{
    public class Abonement
    {
        [Key]
        public int id_abonement { get; set; }

        [Required]
        public string name_abonement { get; set; } = string.Empty;

        [Required]
        public decimal price { get; set; }

        [Required]
        public int duration_days { get; set; }

        public int visit_limit { get; set; }

        public string? description { get; set; }
    }
}