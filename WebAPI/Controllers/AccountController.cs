﻿using Application.Features.AccountContext.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Attirbutes;
using Domain.Enums;
using System.Threading.Tasks;
using Application.Features.AccountContext.Query;
using Application.Features.AccountContext.Dto;
using Application.Common.Dto;

namespace WebAPI.Controllers
{
    public class AccountController : BaseApiControllers
    {
        /// <summary>
        /// Method to register a new user 
        /// </summary>
        /// <param name="request"></param>
        /// <returns> </returns>
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Director)]
        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {

            var res = await Mediator.Send(request);
            return Ok();
        }

        /// <summary>
        /// Login method for user authentication.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>LoginDto</returns>
        [HttpPost]
        public async Task<ActionResult<LoginDto>> Login([FromBody] LoginCommand request)
        {
            var res = await Mediator.Send(request);
            return Ok(res);
        }
        /// <summary>
        /// Handles HTTP GET requests and returns a predefined string response. Need for testing purposes.
        /// </summary>
        /// <remarks>This method requires the caller to be authorized. It responds with an HTTP 200 status
        /// code and a string payload.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing an HTTP 200 response with the string "fdsdfsd".</returns>
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetStr()
        {
            return Ok("fdsdfsd");
        }
        /// <summary>
        /// Retrieves a paginated list of users based on the specified query parameters.
        /// </summary>
        /// <remarks>This endpoint requires the caller to have the <see cref="UserRoleEnum.Director"/>
        /// role.</remarks>
        /// <param name="query">The query parameters used to filter and paginate the list of users. This includes criteria such as page
        /// number, page size, and optional filters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="PagedDto{UserDto}"/> object, which includes the paginated list of users  and associated metadata such
        /// as total count.</returns>
        [HttpGet]
        [AuthorizeRole(UserRoleEnum.Director,UserRoleEnum.Manager)]
        public async Task<ActionResult<PagedDto<UserDto>>> GetAllUsers([FromQuery] GetAllUsersQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }

        /// <summary>
        /// Updates user information based on the provided request.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // need to implement update and delete
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand request)
        {
            var res = await Mediator.Send(request);
            return Ok(res);
        }

        /// <summary>
        /// Deletes a user based on the specified command.
        /// </summary>
        /// <remarks>This action requires the caller to have the <see cref="UserRoleEnum.Director"/> role.
        /// Ensure that the provided <paramref name="request"/> contains valid data.</remarks>
        /// <param name="request">The command containing the details of the user to be deleted.  This must include the necessary identifiers
        /// and any required parameters.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Typically returns a success response
        /// if the user was deleted successfully.</returns>
        [HttpDelete]
        [AuthorizeRole(UserRoleEnum.Director)]
       
        public async Task<IActionResult> DeleteUser([FromQuery] DeleteUserCommand request)
        {
            var res = await Mediator.Send(request);
            return Ok(res);


        }
        /// <summary>
        /// Retrieves the currently authenticated user's information.
        /// </summary>
        /// <remarks>This method requires the caller to be authenticated and authorized. Ensure that the
        /// appropriate  authentication and authorization mechanisms are in place before invoking this method.</remarks>
        /// <param name="query">The query object containing parameters for retrieving the current user's data.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing a <see cref="UserDto"/> object with the authenticated user's
        /// details.</returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser([FromQuery] GetCurrentUser query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }
    }
}
