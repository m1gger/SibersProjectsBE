using Application.Common.Attirbutes;
using Application.Features.CompanyContext.Commands;
using Application.Features.CompanyContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;
using Application.Common.Dto;
using Application.Features.CompanyContext.Dto;


namespace WebAPI.Controllers
{
    public class CompanyController : BaseApiControllers
    {
        /// <summary>
        /// Create a new company
        /// </summary>
        /// <param name="command"></param>
        /// <returns>200 ok</returns>
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Director)]
        
        public async Task<IActionResult> CreateNewCompany([FromBody] AddNewCompanyCommands command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }
        /// <summary>
        /// Get all companies
        /// </summary>
        /// <returns>PagedDto<CompanyDto></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedDto<CompanyDto>>> GetAllCompanies()
        {
            var res = await Mediator.Send(new GetAllCompaniesQuery());
            return Ok(res);
        }

        [HttpDelete]
        [AuthorizeRole(UserRoleEnum.Director)]
        public async Task<IActionResult> DeleteCompany([FromQuery] DeleteCompanyCommand companyCommand) 
        {
            var res =await Mediator.Send(companyCommand);
            return Ok(res);
        }

        [HttpPatch]
        [AuthorizeRole(UserRoleEnum.Director)]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyCommand command) 
        {
            var res = await  Mediator.Send(command);
            return Ok(res);
        }

        // need to implement update and delete
    }
}
