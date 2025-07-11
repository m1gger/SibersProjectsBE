using Application.Common.AbstructClasses;
using Application.Common.Dto;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Features.ProjectContext.Dto;
using Application.Interfaces;
using Application.Common.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.ProjectContext.Query
{
    public class GetAllProjectsQuery : PagedQuery, IRequest<PagedDto<ProjectDto>>
    {
        public string? Search { get; set; } 
        public int? Priorit { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? LeaderId { get; set; }
        public int? CustomerCompanyId { get; set; }
        public int? ContractorCompanyId { get; set; }

    }

    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, PagedDto<ProjectDto>>
    {
        private readonly ISibersDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public GetAllProjectsQueryHandler(ISibersDbContext dbContext, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<PagedDto<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
           var user = await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId, cancellationToken);
             var roles= await _userManager.GetRolesAsync(user);
             var roleEnum = RolesHelper.GetUserRole(roles);


            var query = _dbContext.Projects
                .Include(p => p.Leader)
                .Include(p => p.ProjectCompanies)
                    .ThenInclude(pc => pc.Company)
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable();

            switch (roleEnum) 
            {
                case UserRoleEnum.Employer:
                    query = query.Where(p => p.ProjectUsers.Any(pu => pu.UserId == user.Id));
                    break;
                case UserRoleEnum.Manager:
                    //if manager can be leader and employer in project 
                    query = query.Where(p => p.LeaderUserId == user.Id || p.ProjectUsers.Any(pu => pu.UserId == user.Id));
                    break;
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(p => p.Name.Contains(request.Search) || p.Description.Contains(request.Search));
            }

            if (request.Priorit.HasValue)
            {
                query = query.Where(p => p.Priority == request.Priorit.Value);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(p => p.StartDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(p => p.EndDate <= request.EndDate.Value);
            }

            if (request.LeaderId.HasValue)
            {
                query = query.Where(p => p.LeaderUserId == request.LeaderId.Value);
            }

            if (request.CustomerCompanyId.HasValue)
            {
                query = query.Where(p =>
                    p.ProjectCompanies.Any(pc =>
                        pc.CompanyId == request.CustomerCompanyId.Value &&
                        pc.InProjectEnum == CompanyInProjectEnum.Custumer));
            }

            if (request.ContractorCompanyId.HasValue)
            {
                query = query.Where(p =>
                    p.ProjectCompanies.Any(pc =>
                        pc.CompanyId == request.ContractorCompanyId.Value &&
                        pc.InProjectEnum == CompanyInProjectEnum.Contractor));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var projects = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Priority = p.Priority,
                    LeaderUserName = p.Leader.GetUserFullName(),
                    LeaderUserId = p.LeaderUserId,
                    CustomerCompanyName = p.ProjectCompanies
                        .Where(pc => pc.InProjectEnum == CompanyInProjectEnum.Custumer)
                        .Select(pc => pc.Company.Name)
                        .FirstOrDefault(),
                    ContructorCompanyName = p.ProjectCompanies
                        .Where(pc => pc.InProjectEnum == CompanyInProjectEnum.Contractor)
                        .Select(pc => pc.Company.Name)
                        .FirstOrDefault()
                })
                .ToListAsync(cancellationToken);

            return new PagedDto<ProjectDto>
            {
                Items = projects,
                TotalCount = totalCount
            };
        }
    }

}
