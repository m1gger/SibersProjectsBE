using Application.Common.Attirbutes;
using Application.Features.TaskContext.Commans;
using Application.Features.TaskContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;

namespace WebAPI.Controllers
{
    public class TaskController : BaseApiControllers
    {
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Manager,UserRoleEnum.Director)]
        public async Task<IActionResult> CreateTask([FromBody]AddNewTaskCommand command) 
        {
            var res= await Mediator.Send(command);
            return Ok(res);
        }

        [HttpPatch]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
        [HttpDelete]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]
        public async Task<IActionResult> DeleteTask([FromQuery] DeleteTaskCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        /// update task status needs to be implemented
    }
}
