using Microsoft.AspNetCore.Identity;
using TaskManagement.Model.DTOs;

namespace TaskManagement.Repositiories
{
    public interface IUserRespository
    {
        //List<UserDetailsDTO>? GetUsers();
        //Task<IdentityUser>? GetUserByEmailAsync(string email);
        Task<bool> ApproveUser(string email);
        Task<bool> RejectUser(string email);
        Task<bool> ChangeTrackingStatus(UserTrackingStatusChangeDTO email);
        Task<List<UserDetailsDTO>> GetTrackingUserList();
    }
}
