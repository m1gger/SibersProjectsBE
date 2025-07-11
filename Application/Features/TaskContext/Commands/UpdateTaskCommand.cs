using Application.Interfaces;
using Domain.Enums;
using MediatR;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Application.Common.Helpers;



namespace Application.Features.TaskContext.Commans
{
    public class UpdateTaskCommand : IRequest<Unit>
    {
        public int TaskId { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }
        public int? EmplpyerId { get; set; }
        public TaskStatusEnum? TaskStatus { get; set; } 


    }

    public class UpdateTaskCommandHadnler : IRequestHandler<UpdateTaskCommand, Unit>
    {
        private readonly ISibersDbContext _context;

        public UpdateTaskCommandHadnler(ISibersDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.ProjectTasks
                .Include(t => t.TaskUsers) // если потребуется
                .FirstOrDefaultAsync(t => t.Id == request.TaskId, cancellationToken);

            if (task == null)
                throw new Exception($"Task with Id {request.TaskId} not found.");

            

            if (!string.IsNullOrWhiteSpace(request.Name))
                task.Name = request.Name;

            if (request.Description != null) 
                task.Description = request.Description;

            if (request.StartDate.HasValue)
                task.StartDate = request.StartDate.Value;

            if (request.EndDate.HasValue)
                task.EndDate = request.EndDate.Value;

            if (request.Priority.HasValue)
                task.Priority = request.Priority.Value;

            if (request.TaskStatus.HasValue)
                task.TaskStatus = request.TaskStatus.Value;

            
            if (request.EmplpyerId.HasValue)
            {
                
                var currentEmployee = task.TaskUsers.FirstOrDefault(tu => !tu.IsLeader);
                if (currentEmployee != null)
                {
                    currentEmployee.UserId = request.EmplpyerId.Value;
                }
                else
                {
                    
                    task.TaskUsers.Add(new TaskUser
                    {
                        TaskId = task.Id,
                        UserId = request.EmplpyerId.Value,
                        IsLeader = false
                    });
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }

    public class UpdateTaskCommandValidator : AbstractValidator<UpdateTaskCommand>
    {
        private readonly ISibersDbContext _sibersDbContext;
        private readonly ICurrentUserService _currentUserService;
        private readonly UserManager<User> _userManager;

        public UpdateTaskCommandValidator(ISibersDbContext dbContext,ICurrentUserService currentUserService, UserManager<User> userManager)
        {
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("Task ID is required.");
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate)
                .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
                .WithMessage("Start date must be less than or equal to end date.");
            RuleFor(x => x.TaskId)
                .MustAsync(TaskMustBeAppointedToUser)
                .WithMessage("Task must be appointed to user");

        }

        private async Task<bool> TaskMustBeAppointedToUser(int TaskId, CancellationToken cancellationToken) 
        {
            var user =await _sibersDbContext.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var roles= await _userManager.GetRolesAsync(user);
            var role = RolesHelper.GetUserRole(roles);
            switch (role) 
            {
                case UserRoleEnum.Director:
                    return true;
                case UserRoleEnum.Manager:
                    return await _sibersDbContext.TaskUsers.AnyAsync(x => x.TaskId == TaskId && x.UserId == _currentUserService.UserId, cancellationToken);
                case UserRoleEnum.Employer:
                    return await _sibersDbContext.TaskUsers.AnyAsync(x => x.TaskId == TaskId && x.UserId == _currentUserService.UserId, cancellationToken);
                default:
                    return false;
            }
        }
    }
}
