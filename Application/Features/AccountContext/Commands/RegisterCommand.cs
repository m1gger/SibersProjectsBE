using Application.Features.AccountContext.Dto;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.AccountContext.Commands
{
    public class RegisterCommand : IRequest<LoginDto>
    {
        public string UserName {get; set;}

        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginDto> 
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizeService _authorizeService;
        public RegisterCommandHandler(UserManager<User> userManager,IAuthorizeService authorizeService)
        {
            _userManager = userManager;
            _authorizeService = authorizeService;

        }

        public async Task<LoginDto> Handle(RegisterCommand command, CancellationToken cancellation) 
        {
            var user = new User
            {
                UserName = command.UserName,
                Email = command.Email
            };
            var result = await _userManager.CreateAsync(user, command.Password);
         

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, "EMPLOYER");
            var token = await _authorizeService.GenerateJwtTokenAsync(user);
            return new LoginDto
            {
                Token = token,
                
            };
        }
    }
}
