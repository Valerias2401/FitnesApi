using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class EventGame
    {
        [Key]
        public int id_game { get; set; }

        [Required]
        [ForeignKey("SportEvent")]
        public int id_event { get; set; }

        public virtual SportEvent? SportEvent { get; set; }

        [Required]
        public string first_participant { get; set; } = string.Empty;

        [Required]
        public string second_participant { get; set; } = string.Empty;

        [Column(TypeName = "timestamp without time zone")]
        public DateTime game_time { get; set; }

        public string result { get; set; } = "Не сыграно";
    }
}