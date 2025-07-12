using Application.Common.Dto;
using Application.Features.ProjectContext.Dto;
using MediatR;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;
using Application.Common.Helpers;

namespace Application.Features.ProjectContext.Query
{
    public class GetProjectDetailsQuery : IRequest<ProjectDto>
    {
        public int ProjectId { get; set; }
    }

    public class GetProjectDetailsQueryHandler : IRequestHandler<GetProjectDetailsQuery, ProjectDto>
    {
        private readonly ISibersDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetProjectDetailsQueryHandler(ISibersDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<ProjectDto?> Handle(GetProjectDetailsQuery request, CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
           
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
                return null;

            var projectDto = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Priority = project.Priority,
                LeaderUserId = project.Leader.Id,
                LeaderUserName = project.Leader.UserName,
                CustomerCompamyId = project.ProjectCompanies
                    .FirstOrDefault(pc => pc.InProjectEnum == CompanyInProjectEnum.Custumer)?.Company.Id ?? 0,
                CustomerCompanyName = project.ProjectCompanies
                    .FirstOrDefault(pc => pc.InProjectEnum == CompanyInProjectEnum.Custumer)?.Company.Name ?? string.Empty,
                ContructorCompanyId = project.ProjectCompanies
                    .FirstOrDefault(pc => pc.InProjectEnum == CompanyInProjectEnum.Contractor)?.Company.Id ?? 0,
                ContructorCompanyName = project.ProjectCompanies
                    .FirstOrDefault(pc => pc.InProjectEnum == CompanyInProjectEnum.Contractor)?.Company.Name ?? string.Empty,
                ProjectUsers = project.ProjectUsers.Select(pu => new ProjectUsersDto
                {
                    UserId = pu.User.Id,
                    UserName = pu.User.UserName,
                    Email = pu.User.Email,
                    FullName = pu.User.GetUserFullName()
                }).ToList(),
                ProjectDocuments = project.ProjectDocuments.Select(pd => new ProjectDocumentsDto
                {
                    Id = pd.Id,
                    Name = pd.Description,
                    Path = pd.Path,
                }).ToList()
            };

            return projectDto;
        }
    }
}
