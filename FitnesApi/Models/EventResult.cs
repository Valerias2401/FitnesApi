using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class EventResult
    {
        [Key]
        public int id_result { get; set; }

        [Required]
        [ForeignKey("SportEvent")]
        public int id_event { get; set; }

        public virtual SportEvent? SportEvent { get; set; }

        [Required]
        public string result_text { get; set; } = string.Empty;
    }
}