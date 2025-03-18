using Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository;
using MediatR;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Queries
{
    public record GetAllTasksQuery
    {
        public record Query : IRequest<List<Result>>;

        public record Result
        {
            public int Id { get; set; }
            public required string Name { get; set; }
            public required string Description { get; set; }
            public TaskStatus Status { get; set; }
            public string? AssignedTo { get; set; }
            public string CreatedBy { get; set; } = string.Empty;
            public DateTimeOffset CreatedAt { get; set; }
            public string UpdatedBy { get; set; } = string.Empty;
            public DateTimeOffset UpdatedAt { get; set; }
        }
    }

    public class GetAllTasksQueryHandler(ITaskRepository taskRepository)
        : IRequestHandler<GetAllTasksQuery.Query, List<GetAllTasksQuery.Result>>
    {
        public async Task<List<GetAllTasksQuery.Result>> Handle(GetAllTasksQuery.Query request,
            CancellationToken cancellationToken)
        {
            var tasks = await taskRepository.GetTasks();
            return tasks.Select(task => new GetAllTasksQuery.Result
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                AssignedTo = task.AssignedTo,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                UpdatedBy = task.UpdatedBy,
                UpdatedAt = task.UpdatedAt
            }).ToList();
        }
    }
}
