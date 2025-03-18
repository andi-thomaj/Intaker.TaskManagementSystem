using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Task = Intaker.TaskManagementSystem.domain.TaskManagement.Task;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.infrastructure.EntityFramework
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public required DbSet<Task> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.Load("Domain"));

            var migrationDate = DateTimeOffset.UtcNow;
            modelBuilder.Entity<Task>().HasData([
                new Task
                {
                    Id = 1,
                    Name = "Task 1",
                    Description = "Task 1 Description",
                    Status = TaskStatus.NotStarted,
                    AssignedTo = "User 1",
                    CreatedBy = "System",
                    CreatedAt = migrationDate,
                    UpdatedBy = "System",
                    UpdatedAt = migrationDate
                },
                new Task
                {
                    Id = 2,
                    Name = "Task 2",
                    Description = "Task 2 Description",
                    Status = TaskStatus.NotStarted,
                    AssignedTo = "User 2",
                    CreatedBy = "System",
                    CreatedAt = migrationDate,
                    UpdatedBy = "System",
                    UpdatedAt = migrationDate
                },
                new Task
                {
                    Id = 3,
                    Name = "Task 3",
                    Description = "Task 3 Description",
                    Status = TaskStatus.NotStarted,
                    AssignedTo = "User 3",
                    CreatedBy = "System",
                    CreatedAt = migrationDate,
                    UpdatedBy = "System",
                    UpdatedAt = migrationDate
                },
                new Task
                {
                    Id = 4,
                    Name = "Task 4",
                    Description = "Task 4 Description",
                    Status = TaskStatus.NotStarted,
                    AssignedTo = "User 4",
                    CreatedBy = "System",
                    CreatedAt = migrationDate,
                    UpdatedBy = "System",
                    UpdatedAt = migrationDate
                }
            ]);
        }
    }
}
