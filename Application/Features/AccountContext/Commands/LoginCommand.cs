using Application.Features.AccountContext.Dto;
using Application.Interfaces;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountContext.Commands
{
    public class LoginCommand : IRequest<LoginDto> 
    {
        public string UserName { set; get; }
        public string Password { set; get; }

    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginDto>
    {
        private IAuthorizeService _authorize;
        public LoginCommandHandler(IAuthorizeService authorizeService)
        {
            _authorize = authorizeService;
        }
        public async Task<LoginDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var res = await _authorize.LoginAsync(request.UserName, request.Password);
            return res;
           
        }
    }

    public class LoginValidation : AbstractValidator<LoginCommand>
    {
        private readonly ISibersDbContext _dbContext;
        
        public LoginValidation(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.").WithErrorCode("405");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.").WithErrorCode("405");
            RuleFor(x => x).MustAsync(UserMustExist).WithMessage("User not found ").WithErrorCode("404");
        }

        public async Task<bool> UserMustExist(LoginCommand command, CancellationToken cancellationToken) 
        {
           var res =await _dbContext.Users.AnyAsync(x => x.UserName == command.UserName);
            return res;
        }
    }
}
