using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class SectionEnrollment
    {
        [Key]
        public int id_enrollment { get; set; }

        [Required]
        [ForeignKey("User")]
        public int id_user { get; set; }

        public virtual User? User { get; set; }

        [Required]
        [ForeignKey("SportSection")]
        public int id_section { get; set; }

        public virtual SportSection? SportSection { get; set; }
    }
}