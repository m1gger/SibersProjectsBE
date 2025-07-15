using Application.Common.Attirbutes;
using Application.Common.Dto;
using Application.Features.ProjectContext.Commands;
using Application.Features.ProjectContext.Dto;
using Application.Features.ProjectContext.Query;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ProjectsControllers : BaseApiControllers
    {
        //using post ensted of post because of the List<Id> parameters
        /// <summary>
        /// Removes employers from a project based on the specified command.
        /// </summary>
        /// <remarks>This action requires the caller to have either the Manager or Director role. The
        /// command should include valid project and employer identifiers.</remarks>
        /// <param name="command">The command containing the project identifier and the list of employer IDs to be removed. This parameter
        /// must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, this will be an HTTP 200
        /// response with the operation result.</returns>
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]

        public async Task<IActionResult> DeleteEmpoyersFromProject([FromQuery] RemoveEmployersFromProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        /// <summary>
        /// Adds one or more employers to a specified project.
        /// </summary>
        /// <remarks>This action requires the caller to have either the Manager or Director role. Ensure
        /// that the <paramref name="command"/> contains valid data, including a non-empty list of employer identifiers
        /// and a valid project identifier.</remarks>
        /// <param name="command">The command containing the details of the employers to be added and the target project. This includes the
        /// project identifier and a list of employer identifiers.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Typically, this will be an HTTP 200
        /// response with the result of the operation if successful.</returns>
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]

        public async Task<IActionResult> AddEmployersToProject([FromQuery] AddEmployeToProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        /// <summary>
        /// Creates a new project based on the provided command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>project id</returns>
        [HttpPost]
        [AuthorizeRole( UserRoleEnum.Director)]

        public async Task<IActionResult> CreateNewProjectCommand([FromForm] CreateNewProjectCommand command) 
        {

            var res = await Mediator.Send(command);
            return Ok(res);
        }
        /// <summary>
        /// Gets all projects based on the specified query parameters with pagination and filtering options. without details without docs and users
        /// </summary>
        /// <param name="query"></param>
        /// <returns>PagedDto<ProjectDto></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedDto<ProjectDto>>> GetAllProjects([FromQuery] GetAllProjectsQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }
        /// <summary>
        /// Retrieves detailed information about a project based on the specified query parameters.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized. Ensure the appropriate
        /// authentication and authorization mechanisms are in place before invoking this method.</remarks>
        /// <param name="query">The query parameters used to filter and identify the project details to retrieve. Must not be null and must
        /// contain valid criteria.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="ProjectDto"/> object with the project's details.
        /// Returns an HTTP 200 response if the operation is successful.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ProjectDto>> GetProjectDetails([FromQuery] GetProjectDetailsQuery query) 
        {
            var res = await Mediator.Send(query);
            return Ok(res);

        }
        /// <summary>
        /// Updates an existing project based on the provided command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPatch]
        [AuthorizeRole( UserRoleEnum.Director)]
        public async Task<IActionResult> UpdateProject([FromForm] UpdateProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }



    }
}
