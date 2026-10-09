namespace FitnesApi.Requests
{
    public class CreateEventRequest
    {
        public string name { get; set; } = string.Empty;

        public string? description { get; set; }

        public DateTime event_date { get; set; }

        public int id_court { get; set; }

        public int max_participants { get; set; }
    }
}