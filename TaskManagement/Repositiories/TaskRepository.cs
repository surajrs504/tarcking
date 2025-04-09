using TaskManagement.DataContext;
using TaskManagement.Model.Domain;

namespace TaskManagement.Repositiories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagmentDbContext taskManagmentDbContext;

        public TaskRepository(TaskManagmentDbContext taskManagmentDbContext)
        {
            this.taskManagmentDbContext = taskManagmentDbContext;
        }
        public async Task<TaskDomain> Add(TaskDomain task)
        {
            await taskManagmentDbContext.AddAsync(task);
            await taskManagmentDbContext.SaveChangesAsync();
            return task;
        }
    }
}
