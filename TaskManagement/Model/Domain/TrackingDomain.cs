using System.Numerics;

namespace TaskManagement.Model.Domain
{
    public class TrackingDomain
    {
        public Guid Id { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public double TimeStamp { get; set; }


    }
}
