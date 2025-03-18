using Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Commands;
using Intaker.TaskManagementSystem.api.Controllers.TaskManagement.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await mediator.Send(new GetAllTasksQuery.Query());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var result = await mediator.Send(new GetTaskByIdQuery.Query(id));
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetTaskByName(string name)
        {
            var result = await mediator.Send(new GetTaskByNameQuery.Query(name));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskCommand command)
        {
            var id = await mediator.Send(command);
            var newTask = await mediator.Send(new GetTaskByIdQuery.Query(id));
            return CreatedAtAction(nameof(GetTaskById), new { id }, newTask);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask(UpdateTaskCommand command)
        {
            await mediator.Send(command);
            return NoContent();
        }
    }
}
