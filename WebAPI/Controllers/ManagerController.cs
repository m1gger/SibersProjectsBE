using Application.Features.ManagerContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    public class ManagerController :BaseApiControllers
    {
        // need to implement get all Managers 
        /// <summary>
        /// Get All Managers with pagination and filtering options.
        /// </summary>
        /// <param name="query"></param>
        /// <returns>paged Dto of managers</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllManagers([FromQuery] GetAllManagersQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        

        


    }
}
