using FluentValidation;
using Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository;
using MediatR;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Queries
{
    public record GetTaskByIdQuery
    {
        public record Query(int Id) : IRequest<Result>;

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

    public class GetTaskByIdQueryValidator : AbstractValidator<GetTaskByIdQuery.Query>
    {
        public GetTaskByIdQueryValidator(ITaskRepository taskRepository)
        {
            RuleFor(x => x.Id)
                .NotNull()
                .GreaterThan(0)
                .MustAsync(async (id, _) =>
                {
                    var task = await taskRepository.GetTaskById(id);
                    return task is not null;
                })
                .WithMessage((_, id) => $"Task with ID: {id} doesn't exist");
        }
    }

    public class GetTaskByIdQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByIdQuery.Query, GetTaskByIdQuery.Result>
    {
        public async Task<GetTaskByIdQuery.Result> Handle(GetTaskByIdQuery.Query request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetTaskById(request.Id);
            return new GetTaskByIdQuery.Result
            {
                Id = task!.Id,
                Name = task.Name,
                Description = task.Description,
                Status = task.Status,
                AssignedTo = task.AssignedTo,
                CreatedBy = task.CreatedBy,
                CreatedAt = task.CreatedAt,
                UpdatedBy = task.UpdatedBy,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}
