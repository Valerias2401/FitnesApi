namespace FitnesApi.Requests
{
    public class CreateEventResultRequest
    {
        public int id_event { get; set; }

        public string result_text { get; set; } = string.Empty;
    }
}