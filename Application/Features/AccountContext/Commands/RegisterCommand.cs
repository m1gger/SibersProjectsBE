﻿using Application.Features.AccountContext.Dto;
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
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }


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
                , Name = command?.Name,
                LastName = command?.LastName,
                Patronymic = command?.Patronymic
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
            RuleFor(x => x.UserName)
             .NotEmpty()
             .WithMessage("Username is required.")
             .WithErrorCode("2001");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .WithErrorCode("2002");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Valid email is required.")
                .WithErrorCode("2003");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .WithErrorCode("2004");

            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")
                .WithErrorCode("2005");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required.")
                .WithErrorCode("2006");

            RuleFor(x => x.Email)
                .MustAsync(BeUniqueEmail)
                .WithMessage("Email must be unique.")
                .WithErrorCode("2007");

            RuleFor(x => x.UserName)
                .MustAsync(BeUniqueUserName)
                .WithMessage("Username must be unique.")
                .WithErrorCode("2008");

            RuleFor(x => x.Role)
                .MustAsync(BeValidRole)
                .WithMessage("Role must be valid.")
                .WithErrorCode("2009");

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
