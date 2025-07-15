using Application.Interfaces;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.AccountContext.Commands
{
    public class DeleteUserCommand : IRequest<Unit>
    {
        public int UserId { get; set; }
    }

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly ISibersDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        public DeleteUserCommandHandler(ISibersDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }
        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellation)
        {
            var user = await _dbContext.Users.FindAsync(command.UserId);
          
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellation);
            return Unit.Value;
        }
    }


    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        private readonly ISibersDbContext _dbContext;
        public DeleteUserCommandValidator(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .MustAsync(UserExists).WithMessage("User does not exist.");
        }
        private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        }
    }
}
