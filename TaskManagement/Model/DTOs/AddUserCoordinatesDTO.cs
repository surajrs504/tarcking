using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Model.DTOs
{
    public class AddUserCoordinatesDTO
    {
        public double Lat { get; set; }
        public double Long { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Email{ get; set; }
        public double TimeStampMs { get; set; }
    }
}
