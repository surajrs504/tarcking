using Microsoft.EntityFrameworkCore;
using TaskManagement.DataContext;
using TaskManagement.Model.Domain;

namespace TaskManagement.Repositiories
{
    public class TrackingRepository : ITrackingRepository
    {
        public readonly TaskManagmentDbContext taskManagmentDbContext;
        public TrackingRepository(TaskManagmentDbContext taskManagmentDbContext)
        {
            this.taskManagmentDbContext = taskManagmentDbContext;
        }
        public async Task<TrackingDomain> addCooridinates(TrackingDomain details)
        {
            await this.taskManagmentDbContext.Tracking.AddAsync(details);
            await this.taskManagmentDbContext.SaveChangesAsync();
            return details;
        }

        public async Task<List<TrackingDomain>> getCoordinates()
        {   
            var trackingData = await this.taskManagmentDbContext.Tracking.ToListAsync();
            var sortedTrackingData = trackingData.OrderBy(t => t.TimeStamp).ToList();
            return sortedTrackingData;
        }

        public async Task<TrackingDomain?> getLatestCoordinate()
        {
            var trackingData = await this.taskManagmentDbContext.Tracking
                                            .OrderByDescending(r => r.TimeStamp) // Sort by timestamp in descending order
                                             .FirstOrDefaultAsync();
            return trackingData;
        }
    }
}
