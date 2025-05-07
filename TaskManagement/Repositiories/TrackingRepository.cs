using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.DataContext;
using TaskManagement.Model.Domain;
using TaskManagement.Model.DTOs;

namespace TaskManagement.Repositiories
{
    public class TrackingRepository : ITrackingRepository
    {
        private readonly AppDbContext appDbcontext;
        private readonly UserManager<ApplicationUserDomain> userManger;
        public TrackingRepository( AppDbContext appDbContext, UserManager<ApplicationUserDomain> userManager)
        {
            this.appDbcontext = appDbContext;
            this.userManger = userManager;
        }
        //public readonly TaskManagmentDbContext taskManagmentDbContext;
        //private readonly UserManager<IdentityUser> userManager;
        //public TrackingRepository(TaskManagmentDbContext taskManagmentDbContext, UserManager<IdentityUser> userManager)
        //{
        //    this.taskManagmentDbContext = taskManagmentDbContext;
        //    this.userManager = userManager;
        //}
        //public async Task<TrackingDomain?> addCooridinates(AddUserCoordinatesDTO details)
        //{
        //    var userDetails =await this.userManager.FindByEmailAsync(details.Email);
        //    Console.WriteLine("--------------------");
        //    Console.WriteLine(userDetails.Id);
        //    Console.WriteLine("--------------------");
        //    if (userDetails == null)
        //    {
        //        return null;
        //    }
        //    var coordinatesData = new TrackingDomain{
        //        Lat = details.Lat,
        //        Long = details.Long,
        //       UserId=new Guid(userDetails.Id),
        //        TimeStamp = details.TimeStamp
        //    };

        //    await this.taskManagmentDbContext.Tracking.AddAsync(coordinatesData);
        //    await this.taskManagmentDbContext.SaveChangesAsync();
        //    return coordinatesData;
        //}

        //public async Task<List<TrackingDomain>> getCoordinates()
        //{   
        //    var trackingData = await this.taskManagmentDbContext.Tracking.ToListAsync();
        //    var sortedTrackingData = trackingData.OrderBy(t => t.TimeStamp).ToList();
        //    return sortedTrackingData;
        //}

        //public async Task<TrackingDomain?> getLatestCoordinate()
        //{
        //    var trackingData = await this.taskManagmentDbContext.Tracking
        //                                    .OrderByDescending(r => r.TimeStamp) // Sort by timestamp in descending order
        //                                     .FirstOrDefaultAsync();
        //    return trackingData;
        //}

        //public async Task<TrackingDomain?> GetLatestLocationByEmailAsync(string email)
        //{
        //    Console.WriteLine("helloooooo");
        //    var User= await this.taskManagmentDbContext.User.FirstOrDefaultAsync(u=> u.Email == email);
        //    Console.WriteLine($"---------------user id ------------>>>>{User}" );
        //    var a = await this.taskManagmentDbContext.Tracking
        //             .FirstOrDefaultAsync(l => l.TrackId == User.UserId);
        //    Console.WriteLine($"---------------a value ------------>>>>{a}");
        //    return a;
        //}



        public async Task AddLocationAsync( AddUserCoordinatesDTO dto)
        {
            var user = await this.userManger.FindByEmailAsync(dto.Email);
            var location = new UserLocationDomain
            {
                UserId = user.Id,
                Latitude = dto.Lat,
                Longitude = dto.Long,
                Timestamp = dto.TimeStamp,
                TimeStampMs= dto.TimeStampMs
            };

            this.appDbcontext.UserLocations.Add(location);
            await this.appDbcontext.SaveChangesAsync();
           
        }

        public async Task<List<UserLocationDomain>> GetLocationsByUserIdAsync(string email)
        {
            var user = await this.userManger.FindByEmailAsync(email);
            return await this.appDbcontext.UserLocations
                .Where(l => l.UserId == user.Id)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
    }
}
