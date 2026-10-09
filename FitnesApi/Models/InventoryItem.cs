using System.ComponentModel.DataAnnotations;

namespace FitnesApi.Models
{
    public class InventoryItem
    {
        [Key]
        public int id_inventory { get; set; }

        [Required]
        public string name { get; set; } = string.Empty;

        public string category { get; set; } = string.Empty;

        public int quantity { get; set; }

        public string status { get; set; } = "Исправен";

        public bool need_repair { get; set; }

        public bool purchase_required { get; set; }

        public string? comment { get; set; }
    }
}