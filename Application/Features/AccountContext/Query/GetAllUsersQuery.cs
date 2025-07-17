using Application.Common.AbstructClasses;
using Application.Common.Dto;
using Application.Common.Helpers;
using Application.Features.AccountContext.Dto;
using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Application.Features.AccountContext.Query
{
    public class GetAllUsersQuery : PagedQuery, IRequest<PagedDto<UserDto>>
    {
        public int? ProjectId { get; set; }
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
        public string? Role { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedDto<UserDto>>
    {
        private readonly ISibersDbContext _dbContext;

        public GetAllUsersQueryHandler(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedDto<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var usersQuery = _dbContext.Users.Include(x=>x.ProjectUsers).Include(x=>x.TaskUsers).AsQueryable();


            if (request.ProjectId.HasValue)
            {
                usersQuery = usersQuery.Where(u => u.ProjectUsers.Any(pu => pu.ProjectId == request.ProjectId.Value));
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                usersQuery = usersQuery.Where(u =>
                    u.UserName.Contains(request.Search) ||
                    u.Email.Contains(request.Search) ||
                    u.Name.Contains(request.Search) ||
                    u.LastName.Contains(request.Search));
            }

            int? roleId = null;
            if (!string.IsNullOrEmpty(request.Role))
            {
                roleId = await _dbContext.Set<Role>()
                    .Where(r => r.Name.ToUpper() == request.Role.ToUpper())
                    .Select(r => (int?)r.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (roleId.HasValue)
                {
                    usersQuery = usersQuery.Where(u =>
                        _dbContext.Set<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>()
                        .Where(ur => ur.RoleId == roleId.Value)
                        .Select(ur => ur.UserId)
                        .Contains(u.Id));
                }
                else
                {
                    return new PagedDto<UserDto>
                    {
                        Items = new List<UserDto>(),
                        TotalCount = 0,
                        Page = request.PageNumber,
                        PageSize = request.PageSize
                    };
                }
            }
            // can do it easier with denormalization Users set
            var totalCount = await usersQuery.CountAsync(cancellationToken);

            var userPage = await usersQuery
                .OrderByDescending(u => u.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var userIds = userPage.Select(u => u.Id).ToList();
            // this all for map role from Roles
            var userRoles = await (from ur in _dbContext.Set<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>()
                                   join r in _dbContext.Set<Role>() on ur.RoleId equals r.Id
                                   where userIds.Contains(ur.UserId)
                                   select new { ur.UserId, RoleName = r.Name })
                                  .ToListAsync(cancellationToken);

            var rolesDict = userRoles
                .GroupBy(ur => ur.UserId)
                .ToDictionary(g => g.Key, g => g.Select(x => x.RoleName).FirstOrDefault());

            var items = userPage.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.GetUserFullName(),
                Email = u.Email,
                UserName = u.UserName,
                Role = rolesDict.TryGetValue(u.Id, out var role) ? role : null,
                TaskCount = u.TaskUsers.Count,
                ProjectCount = u.ProjectUsers.Count,

            }).ToList();

            return new PagedDto<UserDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.PageNumber,
                PageSize = request.PageSize
            };
        }
    }
}
