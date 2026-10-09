namespace FitnesBlazor.ApiRequests.Models
{
    public class EventGameListResponse
    {
        public bool status { get; set; }

        public List<EventGameModel> data { get; set; } = new();
    }
}