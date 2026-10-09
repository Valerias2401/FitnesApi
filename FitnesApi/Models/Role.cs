using System.ComponentModel.DataAnnotations;

namespace FitnesApi.Models
{
    public class Role
    {
        [Key]
        public int id_role { get; set; }

        [Required]
        public string name_role { get; set; } = string.Empty;
    }
}