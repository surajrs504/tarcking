using TaskManagement.Model.Domain;

namespace TaskManagement.Repositiories
{
    public interface ITaskRepository
    {
        Task<TaskDomain> Add(TaskDomain task);
    }
}
