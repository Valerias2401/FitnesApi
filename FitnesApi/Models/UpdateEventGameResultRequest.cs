namespace FitnesBlazor.ApiRequests.Models
{
    public class UpdateEventGameResultRequest
    {
        public int id_game { get; set; }

        public string result { get; set; } = string.Empty;
    }
}