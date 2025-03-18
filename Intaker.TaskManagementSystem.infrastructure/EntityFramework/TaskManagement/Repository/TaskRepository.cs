using Microsoft.EntityFrameworkCore;
using IntakerTask = Intaker.TaskManagementSystem.domain.TaskManagement.Task;

namespace Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository
{
    public class TaskRepository(ApplicationDbContext dbContext) : ITaskRepository
    {
        public async Task AddTask(IntakerTask task)
        {
            await dbContext.Tasks.AddAsync(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateTask(IntakerTask task)
        {
            dbContext.Tasks.Update(task);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IntakerTask?> GetTaskById(int id)
            => await dbContext.Tasks.FindAsync(id);

        public async Task<List<IntakerTask>> GetTasks()
            => await dbContext.Tasks.ToListAsync();

        public async Task<IntakerTask?> GetTaskByName(string name)
            => await dbContext.Tasks.FirstOrDefaultAsync(x => x.Name == name);
    }
}
