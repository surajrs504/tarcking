namespace TaskManagement.Model.Domain
{
    public class UserLocationDomain
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUserDomain User { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime Timestamp { get; set; }
        public double TimeStampMs { get; set; } 

    }
}
