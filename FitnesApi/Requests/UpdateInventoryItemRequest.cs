namespace FitnesApi.Requests
{
    public class UpdateInventoryStatusRequest
    {
        public int id_inventory { get; set; }

        public string status { get; set; } = "Исправен";

        public bool need_repair { get; set; }

        public bool purchase_required { get; set; }

        public string? comment { get; set; }
    }
}