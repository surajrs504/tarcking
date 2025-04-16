using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TaskManagement.DataContext
{
    public class TaskManagementAuthDbContext :IdentityDbContext
    {
        public TaskManagementAuthDbContext(DbContextOptions<TaskManagementAuthDbContext> options):base(options)
        {
            
        }

    }
}
