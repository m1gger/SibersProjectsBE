using Application.Common.Attirbutes;
using Application.Features.CompanyContext.Commands;
using Application.Features.CompanyContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;


namespace WebAPI.Controllers
{
    public class CompanyController : BaseApiControllers
    {
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Director)]
        public async Task<IActionResult> CreateNewCompany([FromBody] AddNewCompanyCommands command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCompanies()
        {
            var res = await Mediator.Send(new GetAllCompaniesQuery());
            return Ok(res);
        }
    }
}
