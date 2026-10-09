namespace FitnesBlazor.ApiRequests.Models
{
    public class EventGameModel
    {
        public int id_game { get; set; }

        public int id_event { get; set; }

        public string first_participant { get; set; } = string.Empty;

        public string second_participant { get; set; } = string.Empty;

        public DateTime game_time { get; set; }

        public string result { get; set; } = string.Empty;
    }
}