using FluentValidation;
using Intaker.TaskManagement.Contracts;
using Intaker.TaskManagementSystem.infrastructure.EntityFramework.TaskManagement.Repository;
using MassTransit;
using MediatR;
using TaskStatus = Intaker.TaskManagementSystem.domain.TaskManagement.TaskStatus;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Commands
{
    public record UpdateTaskCommand(
        int Id,
        TaskStatus Status) : IRequest<Unit>;

    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        public UpdateTaskCommandValidator(ITaskRepository taskRepository) 
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

            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }

    public class UpdateTaskCommandHandler(ITaskRepository taskRepository, IPublishEndpoint publishEndpoint) : IRequestHandler<UpdateTaskCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await taskRepository.GetTaskById(request.Id);
            task!.Status = request.Status;
            await taskRepository.UpdateTask(task);

            await publishEndpoint.Publish(new TaskUpdated(task.Id, task.Name, task.Description, task.Status, task.AssignedTo!), cancellationToken);

            return Unit.Value;
        }
    }
}
