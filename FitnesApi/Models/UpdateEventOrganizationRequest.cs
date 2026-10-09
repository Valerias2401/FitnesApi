namespace FitnesBlazor.ApiRequests.Models
{
    public class UpdateEventOrganizationRequest
    {
        public int id_event { get; set; }

        public string bracket_info { get; set; } = string.Empty;

        public string game_schedule { get; set; } = string.Empty;

        public string photo_gallery { get; set; } = string.Empty;
    }
}