namespace FitnesApi.Requests
{
    public class CreateCourtRequest
    {
        public string name { get; set; } = string.Empty;

        public string? sport_type { get; set; }

        public decimal price_per_hour { get; set; }
    }
}