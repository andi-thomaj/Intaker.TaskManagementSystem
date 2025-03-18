using IntakerTask = Intaker.TaskManagementSystem.domain.TaskManagement.Task;

namespace Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository
{
    public interface ITaskRepository
    {
        Task AddTask(IntakerTask task);
        Task UpdateTask(IntakerTask task);
        Task<IntakerTask?> GetTaskById(int id);
        Task<List<IntakerTask>> GetTasks();
        Task<IntakerTask?> GetTaskByName(string name);
    }
}
