using TaskManagement.Model.Domain;
using TaskManagement.Model.DTOs;

namespace TaskManagement.Repositiories
{
    public interface IUserLocationInterface
    {
        Task AddLocationAsync(string userId, UserLocationDTO dto);
        Task<List<UserLocationDomain>> GetLocationsByUserIdAsync(string userId);
    }
}
