﻿using Application.Common.Attirbutes;
using Application.Features.ProjectContext.Commands;
using Application.Features.ProjectContext.Query;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ProjectsControllers : BaseApiControllers
    {
        //using post ensted of post because of the List<Id> parameters
        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]

        public async Task<IActionResult> DeleteEmpoyersFromProject([FromQuery] RemoveEmployersFromProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [AuthorizeRole(UserRoleEnum.Manager, UserRoleEnum.Director)]

        public async Task<IActionResult> AddEmployersToProject([FromQuery] AddEmployeToProjectCommand command)
        {
            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [HttpPost]
        [AuthorizeRole( UserRoleEnum.Director)]

        public async Task<IActionResult> CreateNewProjectCommand([FromForm] CreateNewProjectCommand command) 
        {

            var res = await Mediator.Send(command);
            return Ok(res);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllProjects([FromQuery] GetAllProjectsQuery query)
        {
            var res = await Mediator.Send(query);
            return Ok(res);
        }


    }
}
