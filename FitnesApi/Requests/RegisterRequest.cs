namespace FitnesApi.Requests
{
    public class RegisterRequest
    {
        public string full_name { get; set; } = string.Empty;

        public string email { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;

        public string? phone { get; set; }

        public DateTime? birth_date { get; set; }
    }
}