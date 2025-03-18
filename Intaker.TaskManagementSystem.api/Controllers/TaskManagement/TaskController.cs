using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Intaker.TaskManagementSystem.api.Controllers.TaskManagement
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController(IMediator mediator) : ControllerBase
    {
    }
}
