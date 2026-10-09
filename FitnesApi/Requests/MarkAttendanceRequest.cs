namespace FitnesApi.Requests
{
    public class MarkAttendanceRequest
    {
        public int id_enrollment { get; set; }

        public DateTime attendance_date { get; set; }

        public bool is_present { get; set; }

        public string? comment { get; set; }
    }
}