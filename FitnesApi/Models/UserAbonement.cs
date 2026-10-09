using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class UserAbonement
    {
        [Key]
        public int id_user_abonement { get; set; }

        [Required]
        [ForeignKey("User")]
        public int id_user { get; set; }

        public virtual User? User { get; set; }

        [Required]
        [ForeignKey("Abonement")]
        public int id_abonement { get; set; }

        public virtual Abonement? Abonement { get; set; }

        [Column(TypeName = "date")]
        public DateTime start_date { get; set; }

        [Column(TypeName = "date")]
        public DateTime end_date { get; set; }
    }
}