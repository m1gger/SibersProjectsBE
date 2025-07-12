using Application.Features.AccountContext.Dto;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountContext.Commands
{
    public class RegisterCommand : IRequest<LoginDto>
    {
        public string UserName {get; set;}

        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

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
            await _userManager.AddToRoleAsync(user, command.Role.ToUpper());
            var token = await _authorizeService.GenerateJwtTokenAsync(user);
            return new LoginDto
            {
                Token = token,
                
            };
        }
    }

    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public RegisterCommandValidator(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Valid email is required.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
            RuleFor(x => x.Role).NotEmpty().WithMessage("Role is required.");
            RuleFor(x => x.Email).MustAsync(BeUniqueEmail).WithMessage("Email must be unique.");
            RuleFor(x => x.UserName).MustAsync(BeUniqueUserName).WithMessage("Username must be unique.");
            RuleFor(x => x.Role).MustAsync(BeValidRole).WithMessage("Role must be valid.");

        }
        
        private  async Task<bool> BeUniqueEmail(string email,CancellationToken cancellation)
        {
            return !await _userManager.Users.AnyAsync(u => u.Email == email);
        }
        private async Task<bool> BeUniqueUserName(string userName,CancellationToken cancellation)
        {
            return !await _userManager.Users.AnyAsync(u => u.UserName == userName);
        }
        private async Task<bool> BeValidRole(string role,CancellationToken cancellation)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role.ToUpper());
            return roleExists;
        }




    }
}
