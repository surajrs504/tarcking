using Azure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DataContext;
using TaskManagement.Model.Domain;
using TaskManagement.Model.DTOs;

namespace TaskManagement.Repositiories
{
    public class UserRepository : IUserRespository
    {
        private readonly UserManager<ApplicationUserDomain> userManager;
        private readonly AppDbContext appDbContext;

        public UserRepository(UserManager<ApplicationUserDomain> userManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.appDbContext = appDbContext;
        }

        public async Task<bool> ApproveUser(string email)
        {
            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            user.IsApproved = true;
            user.IsRejected = false;
            user.IsTrackingEnabled = false;
            await  this.appDbContext.SaveChangesAsync();

            return true;

        }

        public async Task<bool> ChangeTrackingStatus(UserTrackingStatusChangeDTO payload)
        {

            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
            if (user == null)
            {
                return false;
            }
            user.IsApproved = true;
            user.IsRejected = false;
            user.IsTrackingEnabled = payload.IsTrackingEnabled;
            await this.appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<UserDetailsDTO>> GetTrackingUserList()
        {
            var users = await this.userManager.Users.Where(u => u.IsTrackingEnabled == true).Select(r => new UserDetailsDTO
            {
                Email=r.Email,
                UserName=r.UserName,
                PhoneNumber=r.PhoneNumber
            }).ToListAsync();

            return users;
        }

        public async Task<bool> RejectUser(string email)
        {

            var user = await this.userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }
            user.IsApproved = false;
            user.IsRejected = true;
            user.IsTrackingEnabled = false;
            await this.appDbContext.SaveChangesAsync();

            return true;
        }
        //public List<UserDetailsDTO>? GetUsers()
        //{
        //    var users =  this.userManager.Users.ToList();
        //    var userDetailsList = new List<UserDetailsDTO>();
        //    foreach (var user in users)
        //    {
        //        var userDto = new UserDetailsDTO
        //        {
        //            UserName = user.UserName,
        //            Email = user.Email,
        //           PhoneNumber=user.PhoneNumber
        //        };
        //        userDetailsList.Add(userDto);
        //    }

        //    if (users == null)
        //    {
        //        return null;
        //    }
        //    return userDetailsList;
        //}
        //public async Task<IdentityUser>? GetUserByEmailAsync(string email)
        //{
        //    var user = await this.userManager.Users
        //                         .FirstOrDefaultAsync(u => u.Email == email);
        //    if (user == null)
        //    {
        //        return null;
        //    }
        //    return user;
        //}
    }
}

   
