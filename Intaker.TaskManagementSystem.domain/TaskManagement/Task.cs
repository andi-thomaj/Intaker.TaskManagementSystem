using Intaker.TaskManagementSystem.domain.Shared;

namespace Intaker.TaskManagementSystem.domain.TaskManagement
{
    public class Task : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
        public string? AssignedTo { get; set; }
    }
}
