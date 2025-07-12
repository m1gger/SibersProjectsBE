using Application.Common.Dto;
using Application.Features.EmployeContext.Dto;
using Application.Features.EmployeContext.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class EmployersController : BaseApiControllers
    {
        /// <summary>
        /// Retrieves a list of employers based on the specified query parameters.
        /// </summary>
        /// <remarks>This endpoint requires authentication and authorization. Ensure the user has the
        /// necessary permissions to access employer data.</remarks>
        /// <param name="query">The query parameters used to filter and retrieve the list of employers. This parameter must not be null.</param>
        /// <returns>An <see cref="IActionResult"/> containing the list of employers that match the query parameters. The
        /// response is typically returned as an HTTP 200 OK status with the data, or an appropriate error status if the
        /// request fails.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<PagedDto<EmpoyerDto>>> GetEmployers([FromQuery] GetAllEmployersQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }
    }
}
