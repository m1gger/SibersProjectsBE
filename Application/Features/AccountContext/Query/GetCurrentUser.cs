using MediatR;
using Application.Features.AccountContext.Dto;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Application.Common.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Domain.GenericExtensions;

namespace Application.Features.AccountContext.Query
{
    public class GetCurrentUser : IRequest<UserDto>
    {
    }

    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUser, UserDto>
    {
        private readonly ISibersDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;
        public GetCurrentUserHandler(ISibersDbContext context, ICurrentUserService currentUserService,UserManager<User> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }
        public async Task<UserDto> Handle(GetCurrentUser request, CancellationToken cancellationToken)
        {

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
                var roles =await _userManager.GetRolesAsync(user);
            var roleEnum= RolesHelper.GetUserRole(roles);

            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = user.GetUserFullName(),
                Email = user.Email,
                UserName = user.UserName,
                Role = roleEnum.GetDescription()
            };


            return userDto;
        }
    }

}
