using FluentValidation;
using Intaker.TaskManagement.Contracts;
using Intaker.TaskManagementSystem.domain.TaskManagement.Configurations;
using Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository;
using MassTransit;
using MediatR;
using IntakerTask = Intaker.TaskManagementSystem.domain.TaskManagement.Task;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Commands
{
    public record CreateTaskCommand(
        string Name,
        string Description,
        TaskStatus Status,
        string? AssignedTo) : IRequest<int>;

    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator(ITaskRepository taskRepository)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(TaskConfiguration.Settings.NameMinLength)
                .MaximumLength(TaskConfiguration.Settings.NameMaxLength)
                .MustAsync(async (name, _) =>
                {
                    var task = await taskRepository.GetTaskByName(name);
            
                    return task is null;
                })
                .WithMessage((_, name) => $"Task name: {name} already exists.");
            
            RuleFor(x => x.Description)
                .NotEmpty()
                .MinimumLength(TaskConfiguration.Settings.DescriptionMinLength)
                .MaximumLength(TaskConfiguration.Settings.DescriptionMaxLength);
            
            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }

    public class CreateTaskCommandHandler(ITaskRepository taskRepository, IPublishEndpoint publishEndpoint) : IRequestHandler<CreateTaskCommand, int>
    {
        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var insertionTime = DateTimeOffset.UtcNow;
            var task = new IntakerTask
            {
                Name = request.Name,
                Description = request.Description,
                Status = request.Status,
                AssignedTo = request.AssignedTo,
                CreatedBy = "System",
                CreatedAt = insertionTime,
                UpdatedBy = "System",
                UpdatedAt = insertionTime
            };

            await taskRepository.AddTask(task);
            var taskAdded = await taskRepository.GetTaskByName(task.Name);

            await publishEndpoint.Publish(new TaskCreated(taskAdded!.Id, taskAdded.Name, taskAdded.Description, taskAdded.Status, taskAdded.AssignedTo!), cancellationToken);

            return taskAdded.Id;
        }
    }
}
