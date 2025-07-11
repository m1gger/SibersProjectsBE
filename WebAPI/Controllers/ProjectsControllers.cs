using Application.Features.ProjectContext.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ProjectsControllers : BaseApiControllers
    {
        //using post ensted of post because of the List<Id> parameters
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteEmpoyersFromProject([FromQuery] RemoveEmployersFromProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddEmployersToProject([FromQuery] AddEmployeToProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        public async Task<IActionResult> CreateNewProjectCommand([FromForm] CreateNewProjectCommand command) 
        {

            var res = await Mediator.Send(command);
            return Ok(res);
        }
    }
}
