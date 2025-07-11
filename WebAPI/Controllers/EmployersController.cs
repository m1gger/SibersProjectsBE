using Application.Features.EmployeContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class EmployersController : BaseApiControllers
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEmployers([FromQuery] GetAllEmployersQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }
    }
}
