using Application.Common.AbstructClasses;
using Application.Common.Dto;
using Application.Common.Helpers;
using Application.Features.TaskContext.Dto;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.GenericExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.TaskContext.Query
{
    public class GetAllTasksQuery : PagedQuery, IRequest<PagedDto<TaskDto>>
    {
        public int? ProjectId { get; set; }
        public string? Search { get; set; }
        public int? UserId { get; set; }
        public TaskStatusEnum? TaskStatusEnum { get; set; }
        public int? Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }

    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, PagedDto<TaskDto>>
    {
        private readonly ISibersDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public GetAllTasksQueryHandler(ISibersDbContext context, ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            _context = context;
            _currentUserService = currentUserService;
            _userManager = userManager;
        }

        public async Task<PagedDto<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var query = _context.ProjectTasks.AsQueryable();
            query = query.Include(x => x.Project)
                .Include(x => x.TaskUsers).ThenInclude(x => x.User);
           




            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var roles = await _userManager.GetRolesAsync(user);
            var role = RolesHelper.GetUserRole(roles);
            switch (role)
            {
                case UserRoleEnum.Employer:
                    query = query.Where(x => x.TaskUsers.Any(x => x.UserId == _currentUserService.UserId));
                    break;
                
                case UserRoleEnum.Manager:
                    query = query.Where(x => x.TaskUsers.Any(x => x.UserId == _currentUserService.UserId)|| x.Project.LeaderUserId==_currentUserService.UserId);
                   
                    break;
            }

            if (request.ProjectId.HasValue)
            {
                query = query.Where(t => t.ProjectId == request.ProjectId.Value);
            }

            if (!string.IsNullOrEmpty(request.Search))
            {
                query = query.Where(t => t.Name.Contains(request.Search) || t.Description.Contains(request.Search));
            }

            if (request.UserId.HasValue)
            {
                // Check if the user is a leader in any task
               
                switch (role) 
                {
                    case UserRoleEnum.Employer:
                        query = query.Where(x => x.TaskUsers.Any(x => x.UserId == _currentUserService.UserId));
                        break;
                    case UserRoleEnum.Director:
                        query = query.Where(x => x.TaskUsers.Any(x => x.UserId == request.UserId));
                        break;
                    case UserRoleEnum.Manager:
                        query = query.Where(x => x.TaskUsers.Any(x => x.UserId == request.UserId));
                        break;


                }
                query = query.Where(x => x.TaskUsers.Any(x => x.UserId == request.UserId));
            }

            if (request.TaskStatusEnum.HasValue)
            {
                query = query.Where(t => t.TaskStatus == request.TaskStatusEnum.Value);
            }

            if (request.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == request.Priority.Value);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(t => t.StartDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(t => t.EndDate <= request.EndDate.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var tasks = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    TaskStatus = t.TaskStatus.GetDescription(),
                    Priority = t.Priority,
                    EmployeeName=t.TaskUsers.Where(x => x.IsLeader==false).Select(x => x.User.Name).FirstOrDefault() ?? string.Empty,
                    EmployeeUserId = t.TaskUsers.Where(x => x.IsLeader == false).Select(x => x.UserId).FirstOrDefault(),
                    LeaderName = t.TaskUsers.Where(x => x.IsLeader).Select(x => x.User.Name).FirstOrDefault() ?? string.Empty,
                    LeaderUserId = t.TaskUsers.Where(x => x.IsLeader).Select(x => x.UserId).FirstOrDefault(),
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    ProjectId=t.ProjectId,
                    ProjectName=t.Project.Name, 
                })
                .ToListAsync(cancellationToken);

            return new PagedDto<TaskDto>(tasks, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
