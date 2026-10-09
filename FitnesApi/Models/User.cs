using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnesApi.Models
{
    public class User
    {
        [Key]
        public int id_user { get; set; }

        [Required]
        public string full_name { get; set; } = string.Empty;

        [Required]
        public string email { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;

        public string? phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? birth_date { get; set; }

        [Required]
        [ForeignKey("Role")]
        public int id_role { get; set; }

        public decimal balance { get; set; }

        public virtual Role? Role { get; set; }
    }
}