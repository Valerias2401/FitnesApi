namespace FitnesApi.Requests
{
    public class CreateEventGameRequest
    {
        public int id_event { get; set; }

        public string first_participant { get; set; } = string.Empty;

        public string second_participant { get; set; } = string.Empty;

        public DateTime game_time { get; set; }
    }
}