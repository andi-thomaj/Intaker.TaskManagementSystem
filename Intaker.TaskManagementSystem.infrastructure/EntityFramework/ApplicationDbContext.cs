﻿using System.Reflection;
using Intaker.TaskManagementSystem.domain.Shared;
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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Task).Assembly);

            var migrationDate = new DateTimeOffset(2025, 3, 18, 15, 30, 45, TimeSpan.Zero);
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

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entities = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.State is EntityState.Modified or EntityState.Added);

            foreach (var entity in entities)
            {
                entity.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
