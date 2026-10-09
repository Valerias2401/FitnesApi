using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class EventParticipant
    {
        [Key]
        public int id_participant { get; set; }

        [Required]
        [ForeignKey("SportEvent")]
        public int id_event { get; set; }

        public virtual SportEvent? SportEvent { get; set; }

        [Required]
        [ForeignKey("User")]
        public int id_user { get; set; }

        public virtual User? User { get; set; }
    }
}