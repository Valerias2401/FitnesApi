using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class SportEvent
    {
        [Key]
        public int id_event { get; set; }

        [Required]
        public string name { get; set; } = string.Empty;

        public string? description { get; set; }

        [Column(TypeName = "timestamp without time zone")]
        public DateTime event_date { get; set; }

        [Required]
        [ForeignKey("Court")]
        public int id_court { get; set; }

        public virtual Court? Court { get; set; }

        public int max_participants { get; set; }

        public string bracket_info { get; set; } = string.Empty;

        public string game_schedule { get; set; } = string.Empty;

        public string photo_gallery { get; set; } = string.Empty;

        public string status { get; set; } = "planned";
    }
}