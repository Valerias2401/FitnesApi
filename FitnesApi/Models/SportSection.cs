using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class SportSection
    {
        [Key]
        public int id_section { get; set; }

        [Required]
        public string name { get; set; } = string.Empty;

        [ForeignKey("Trainer")]
        public int? trainer_id { get; set; }

        public virtual User? Trainer { get; set; }

        public int? max_participants { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime? schedule_time { get; set; }
    }
}