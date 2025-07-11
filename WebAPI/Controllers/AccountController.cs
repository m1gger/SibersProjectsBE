using Application.Features.AccountContext.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class AccountController : BaseApiControllers
    {
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {

            var res = await Mediator.Send(request);
            return Ok(res);
        }
    }
}
