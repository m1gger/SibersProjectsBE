using Domain.Entities;
using Domain.Enums;
using MediatR;
using Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.Common.Helpers;


namespace Application.Features.TaskContext.Commans
{
    public class AddNewTaskCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int ProjectId { get; set; }
        public int? LeaderId { get; set; }
        public int EmployeeId { get; set; }

    }

    public class AddNewTaskCommandHandler : IRequestHandler<AddNewTaskCommand, int> 
    {
        private readonly ISibersDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly UserManager<User> _userManager;
        public AddNewTaskCommandHandler(ISibersDbContext context, ICurrentUserService currentUser,UserManager<User> userManager)
        {
            _context = context;
            _currentUser = currentUser;
            _userManager = userManager;
        }
        public async Task<int> Handle(AddNewTaskCommand request, CancellationToken cancellationToken)
        {
             var task = new ProjectTask
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Priority = request.Priority,
                ProjectId = request.ProjectId,
                TaskStatus = TaskStatusEnum.NotStarted,
            };
            var taskUser = new TaskUser
            {
                Task = task,
                UserId = request.EmployeeId,
                IsLeader = false

            };
            var user=await _context.Users.FirstOrDefaultAsync(u => u.Id == _currentUser.UserId);
            var roles = await _userManager.GetRolesAsync(user);
            var role= RolesHelper.GetUserRole(roles);
            switch (role)
            {
                case UserRoleEnum.Director:
                    if (request.LeaderId.HasValue)
                    {
                        var taskLeader1 = new TaskUser
                        {
                            Task = task,
                            UserId = request.LeaderId.Value,
                            IsLeader = true

                        };
                        task.TaskUsers.Add(taskLeader1);
                    }
                    else 
                    {
                        throw new Exception("LeaderId is required for Director role");
                    }
                    break;
                case UserRoleEnum.Manager:
                    var taskLeader = new TaskUser
                    {
                        Task = task,
                        UserId =_currentUser.UserId.Value,
                        IsLeader = true

                    };
                    task.TaskUsers.Add(taskLeader);
                    break;
                
                default:
                    throw new Exception("Unknown user role");
            }


            task.TaskUsers.Add(taskUser);
            
            _context.ProjectTasks.Add(task);
            _context.TaskUsers.AddRange(task.TaskUsers);
            await _context.SaveChangesAsync(cancellationToken);
            return task.Id;
        }
    }
}
