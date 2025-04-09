using Microsoft.EntityFrameworkCore;
using TaskManagement.Model.Domain;

namespace TaskManagement.DataContext
{
    public class TaskManagmentDbContext :DbContext
    {
        public TaskManagmentDbContext(DbContextOptions<TaskManagmentDbContext> dbContextOptions):base(dbContextOptions)
        {
            
        }

        public DbSet<TaskDomain> Task { get; set; }
        public DbSet<TeamDomain> Team { get; set; }
        public DbSet<UserDomain> User { get; set; }
        public DbSet<TrackingDomain> Tracking { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Task - User relationship
            modelBuilder.Entity<TaskDomain>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction); // No cascading delete

          

            // Configure Task - Team relationship
            modelBuilder.Entity<TaskDomain>()
                .HasOne(t => t.Team)
                .WithMany()
                .HasForeignKey(t => t.TeamId)
                .OnDelete(DeleteBehavior.NoAction); // No cascading delete

            //// Configure User - Team relationship (if needed)
            //modelBuilder.Entity<UserDomain>()
            //    .HasOne(u => u.Team)
            //    .WithMany(t => t.Users)
            //    .HasForeignKey(u => u.TeamId)
            //    .OnDelete(DeleteBehavior.NoAction); // No cascading delete
        }
    }
}
