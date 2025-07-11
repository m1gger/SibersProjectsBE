using Application.Features.TaskContext.Commans;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class TaskController : BaseApiControllers
    {
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTask([FromBody]AddNewTaskCommand command) 
        {
            var res= await Mediator.Send(command);
            return Ok(res);
        }
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromQuery] DeleteTaskCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
    }
}
