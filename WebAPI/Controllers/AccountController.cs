using Application.Features.AccountContext.Commands;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand request)
        {
            var res = await Mediator.Send(request);
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStr() 
        {
            return Ok("fdsdfsd");
        }
    }
}
