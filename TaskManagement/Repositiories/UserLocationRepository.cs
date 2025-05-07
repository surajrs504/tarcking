using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.DataContext;
using TaskManagement.Model.Domain;
using TaskManagement.Model.DTOs;

namespace TaskManagement.Repositiories
{
    public class UserLocationRepository:IUserLocationInterface
    {
        private readonly AppDbContext _context;

        public UserLocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddLocationAsync(string userId, UserLocationDTO dto)
        {
            var location = new UserLocationDomain
            {
                UserId = userId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                Timestamp = DateTime.UtcNow
            };

            _context.UserLocations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserLocationDomain>> GetLocationsByUserIdAsync(string userId)
        {
            return await _context.UserLocations
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .ToListAsync();
        }
    }
}
