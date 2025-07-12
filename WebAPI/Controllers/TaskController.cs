using Application.Common.Attirbutes;
using Application.Features.TaskContext.Commans;
using Application.Features.TaskContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;
using Application.Common.Dto;
using Application.Features.TaskContext.Dto;

namespace WebAPI.Controllers
{
    public class TaskController : BaseApiControllers
    {
        /// <summary>
        /// Creates a new task based on the provided command and returns the result.
        /// </summary>
        /// <remarks>This method is restricted to users with the roles of Manager or Director.  The task
        /// details must be provided in the <paramref name="command"/> parameter.</remarks>
        /// <param name="command">The command containing the details of the task to be created.</param>
        /// <returns>An <see cref="IActionResult"/> containing the result of the task creation operation.</returns>
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Manager,UserRoleEnum.Director)]
        public async Task<IActionResult> CreateTask([FromBody]AddNewTaskCommand command) 
        {
            var res= await Mediator.Send(command);
            return Ok(res);
        }
        /// <summary>
        /// Updates an existing task with the specified details.
        /// </summary>
        /// <remarks>This endpoint requires authorization and is restricted to users with the roles <see
        /// cref="UserRoleEnum.Manager"/> or <see cref="UserRoleEnum.Director"/>.</remarks>
        /// <param name="command">The command containing the updated task details. This must include all required fields for the update
        /// operation.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns a success response with the
        /// updated task details if the operation completes successfully.</returns>
        [HttpPatch]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
        /// <summary>
        /// Deletes a task based on the specified command.
        /// </summary>
        /// <remarks>This action requires the caller to have either the Manager or Director role. Ensure
        /// that the <paramref name="command"/> contains valid data, as incomplete or invalid  parameters may result in
        /// a failed operation.</remarks>
        /// <param name="command">The command containing the details of the task to be deleted.  This must include the necessary identifiers
        /// and parameters required for deletion.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically, a success response is
        /// returned if the task is deleted successfully.</returns>
        [HttpDelete]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]
        public async Task<IActionResult> DeleteTask([FromQuery] DeleteTaskCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
        /// <summary>
        /// Retrieves all tasks based on the specified query parameters.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized. The query parameters can be used to
        /// filter or modify the results.</remarks>
        /// <param name="query">The query parameters used to filter or customize the task retrieval. Cannot be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of tasks that match the query parameters.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedDto<TaskDto>>> GetAllTasks([FromQuery] GetAllTasksQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }
        /// <summary>
        /// Updates the status of a task based on the provided command.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized. Ensure that the user has the
        /// necessary permissions to update the task status.</remarks>
        /// <param name="command">The command containing the task identifier and the new status to apply. The command must not be null and
        /// must include valid task details.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see langword="Ok"/> with the
        /// updated task details if the operation succeeds.</returns>
        [HttpPatch]
        [Authorize]
        public async Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskCommand command) 
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
      
    }
}
