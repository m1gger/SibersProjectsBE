using Application.Features.ManagerContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace WebAPI.Controllers
{
    public class ManagerController :BaseApiControllers
    {
        // need to implement get all Managers 
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllManagers([FromQuery] GetAllManagersQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        

        


    }
}
