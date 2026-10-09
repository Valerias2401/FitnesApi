namespace FitnesApi.Requests
{
    public class CreateBookingRequest
    {
        public int id_user { get; set; }

        public int id_court { get; set; }

        public DateTime booking_date { get; set; }

        public TimeSpan start_time { get; set; }

        public TimeSpan end_time { get; set; }
    }
}