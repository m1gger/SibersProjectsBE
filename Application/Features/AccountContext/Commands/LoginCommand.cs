using Application.Features.AccountContext.Dto;
using Application.Interfaces;
using MediatR;

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
}
