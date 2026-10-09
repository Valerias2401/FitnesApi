namespace FitnesApi.Requests
{
    public class CreateSectionRequest
    {
        public string name { get; set; } = string.Empty;

        public int? trainer_id { get; set; }

        public int? max_participants { get; set; }

        public DateTime? schedule_time { get; set; }
    }
}