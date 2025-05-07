using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Model.Domain;

namespace TaskManagement.DataContext
{
    public class AppDbContext:IdentityDbContext<ApplicationUserDomain>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<UserLocationDomain> UserLocations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure UserLocation foreign key to ApplicationUser
            builder.Entity<UserLocationDomain>()
                .HasOne(ul => ul.User)
                .WithMany() // Or use `.WithMany(u => u.Locations)` if you want a collection in ApplicationUser
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: delete locations if user is deleted

            // Optional: Add indexes or constraints
            builder.Entity<UserLocationDomain>()
                .HasIndex(ul => ul.UserId);
        }
    }
}
