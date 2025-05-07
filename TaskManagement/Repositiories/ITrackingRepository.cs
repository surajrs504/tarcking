using System.Collections.ObjectModel;
using TaskManagement.Model.Domain;
using TaskManagement.Model.DTOs;

namespace TaskManagement.Repositiories
{
    public interface ITrackingRepository
    {
        //    Task<TrackingDomain> addCooridinates(AddUserCoordinatesDTO details);
        //    Task<List<TrackingDomain>> getCoordinates();
        //    Task<TrackingDomain?> getLatestCoordinate();
        //    Task<TrackingDomain?> GetLatestLocationByEmailAsync(string email);
        Task AddLocationAsync( AddUserCoordinatesDTO dto);
        Task<List<UserLocationDomain>> GetLocationsByUserIdAsync(string userId);
    }
}
