using System.Collections.ObjectModel;
using TaskManagement.Model.Domain;

namespace TaskManagement.Repositiories
{
    public interface ITrackingRepository
    {
        Task<TrackingDomain> addCooridinates(TrackingDomain details);
        Task<List<TrackingDomain>> getCoordinates();
        Task<TrackingDomain?> getLatestCoordinate();
    }
}
