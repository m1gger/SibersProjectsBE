using MediatR;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Domain.Entities;
using Application.Common.Helpers;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.TaskContext.Commans
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public int TaskId { get; set; }

    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ISibersDbContext _context;
        public DeleteTaskCommandHandler(ISibersDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.ProjectTasks.FindAsync(request.TaskId);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }









    public class DeleteTaskCommansValidator : AbstractValidator<DeleteTaskCommand>
    {
        private readonly ISibersDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ICurrentUserService _currentUserService;
        public DeleteTaskCommansValidator(ISibersDbContext context, UserManager<User> userManager, ICurrentUserService currentUserService)
        {
            _context = context;
            _userManager = userManager;
            _currentUserService = currentUserService;
            RuleFor(x => x.TaskId).NotEmpty().WithMessage("TaskId is required.");
            RuleFor(x => x.TaskId)
                .MustAsync(TaskMustBeAppointedToUser)
                .WithMessage("You do not have permission to delete this task.");
        }

        private async Task<bool> TaskMustBeAppointedToUser(int TaskId, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == _currentUserService.UserId);
            var roles = await _userManager.GetRolesAsync(user);
            var role = RolesHelper.GetUserRole(roles);
            switch (role)
            {
                case UserRoleEnum.Director:
                    return true;
                case UserRoleEnum.Manager:
                    return await _context.TaskUsers.AnyAsync(x => x.TaskId == TaskId && x.UserId == _currentUserService.UserId, cancellationToken);
                case UserRoleEnum.Employer:
                    return false;
                default:
                    return false;
            }
        }

    }


}
