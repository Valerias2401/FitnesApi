using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class SectionAttendance
    {
        [Key]
        public int id_attendance { get; set; }

        [Required]
        [ForeignKey("SectionEnrollment")]
        public int id_enrollment { get; set; }

        public virtual SectionEnrollment? SectionEnrollment { get; set; }

        [Column(TypeName = "date")]
        public DateTime attendance_date { get; set; }

        public bool is_present { get; set; }

        public string? comment { get; set; }
    }
}