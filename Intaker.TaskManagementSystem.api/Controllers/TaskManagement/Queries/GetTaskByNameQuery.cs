using FluentValidation;
using Intaker.TaskManagementSystem.domain.TaskManagement.Configurations;
using Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository;
using MediatR;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Queries
{
    public record GetTaskByNameQuery
    {
        public record Query(string Name) : IRequest<Result>;

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

    public class GetTaskByNameQueryValidator : AbstractValidator<GetTaskByNameQuery.Query>
    {
        public GetTaskByNameQueryValidator(ITaskRepository taskRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(TaskConfiguration.Settings.NameMinLength)
                .MaximumLength(TaskConfiguration.Settings.NameMaxLength)
                .MustAsync(async (name, _) =>
                {
                    var task = await taskRepository.GetTaskByName(name);
                    return task is not null;
                })
                .WithMessage((_, name) => $"Task with name: {name} doesn't exist.");
        }
    }

    public class GetTaskByNameQueryHandler(ITaskRepository taskRepository) : IRequestHandler<GetTaskByNameQuery.Query, GetTaskByNameQuery.Result>
    {
        public async Task<GetTaskByNameQuery.Result> Handle(GetTaskByNameQuery.Query request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetTaskByName(request.Name);
            return new GetTaskByNameQuery.Result
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
