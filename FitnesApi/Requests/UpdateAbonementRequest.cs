namespace FitnesApi.Requests
{
    public class UpdateAbonementRequest
    {
        public int id_abonement { get; set; }

        public string name_abonement { get; set; } = string.Empty;

        public string type { get; set; } = "Месячный";

        public decimal price { get; set; }

        public int duration_days { get; set; }

        public int visit_limit { get; set; }

        public string? description { get; set; }
    }
}