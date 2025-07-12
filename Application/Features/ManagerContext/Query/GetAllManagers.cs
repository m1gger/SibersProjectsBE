using Application.Common.AbstructClasses;
using MediatR;
using Application.Common.Dto;
using Application.Features.EmployeContext.Dto;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Common.Helpers;
using Application.Features.ManagerContext.Dto;

namespace Application.Features.ManagerContext.Query
{
    public class GetAllManagersQuery : PagedQuery, IRequest<PagedDto<ManagerDto>>
    {
        public int? ProjectId { get; set; }
        public string? Search { get; set; } = string.Empty;
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }

    }

    public class GetAllManagersQueryHandler : IRequestHandler<GetAllManagersQuery, PagedDto<ManagerDto>>
    {
        private readonly ISibersDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        public GetAllManagersQueryHandler(ISibersDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<PagedDto<ManagerDto>> Handle(GetAllManagersQuery query, CancellationToken cancellationToken)
        {
            // 1. Найти Id роли EMPLOYER
            var employerRoleId = await _dbContext.Set<Role>()
                .Where(r => r.Name == "MANAGER")
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (employerRoleId == 0)
            {
                return new PagedDto<ManagerDto>
                {
                    Items = new List<ManagerDto>(),
                    TotalCount = 0,
                    Page = query.PageNumber,
                    PageSize = query.PageSize
                };
            }

            // 2. Построить основной запрос
            var employers = _dbContext.Users
                .Where(u => _dbContext.Set<IdentityUserRole<int>>()
                    .Where(ur => ur.RoleId == employerRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(u.Id));
            if (query.ProjectId.HasValue)
            {
                employers = employers.Where(u => u.ProjectUsers.Any(pu => pu.ProjectId == query.ProjectId.Value));
            }

            // 3. Поиск
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                employers = employers.Where(u =>
                    u.Name.Contains(query.Search) ||
                    u.LastName.Contains(query.Search) ||
                    u.UserName.Contains(query.Search) ||
                    u.Email.Contains(query.Search));
            }


            // 4. Сортировка
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                employers = query.IsDescending.HasValue && query.IsDescending.Value
                    ? employers.OrderByDescending(u => EF.Property<object>(u, query.SortBy))
                    : employers.OrderBy(u => EF.Property<object>(u, query.SortBy));
            }
            else
            {
                employers = employers.OrderBy(u => u.Id);
            }

            // 5. Подсчитать всего
            var totalCount = await employers.CountAsync(cancellationToken);

            // 6. Пагинация
            var skip = (query.PageNumber - 1) * query.PageSize;

            var items = await employers
                .Skip(skip)
                .Take(query.PageSize)
                .Select(u => new ManagerDto
                {
                    Id = u.Id,
                    FullName = u.GetUserFullName(),
                    Email = u.Email,
                    UserName = u.UserName,
                })
                .ToListAsync(cancellationToken);

            return new PagedDto<ManagerDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = query.PageNumber,
                PageSize = query.PageSize
            };
        }

    }
}
