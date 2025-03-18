using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagement.Contracts
{
    public record TaskUpdated(
        int Id,
        string Name,
        string Description,
        TaskStatus Status,
        string AssignedTo);
}
