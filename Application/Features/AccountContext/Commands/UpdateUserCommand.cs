using MediatR;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using FluentValidation;

namespace Application.Features.AccountContext.Commands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Patronymic { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }



    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly ISibersDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;
        public UpdateUserCommandHandler(ISibersDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellation)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId, cancellation);

            if (!string.IsNullOrEmpty(command.Name))
                user.Name = command.Name;

            if (!string.IsNullOrEmpty(command.LastName))
                user.LastName = command.LastName;

            if (!string.IsNullOrEmpty(command.Email))
                user.Email = command.Email;

            if (!string.IsNullOrEmpty(command.Patronymic))
                user.Patronymic = command.Patronymic;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync(cancellation);
            return Unit.Value;
        }
    }


    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ISibersDbContext _dbContext;
        public UpdateUserCommandValidator(ICurrentUserService currentUserService, ISibersDbContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            RuleFor(x => x)
                .Must(x => x.Name != null || x.LastName != null || x.Email != null || x.Patronymic != null)
                .WithMessage("At least one field must be provided.");

            RuleFor(x => x.Email)
                .MustAsync(IsEmailUnique)
                .When(x=>!string.IsNullOrEmpty( x.Email))
                .WithMessage("Email must be unique.");
            RuleFor(x => x)
                .MustAsync(UserMustExist)
                .WithMessage("User must exist.");



        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email == email && u.Id != _currentUserService.UserId, cancellationToken);
        }

        private async Task<bool> UserMustExist(UpdateUserCommand command,CancellationToken cancellationToken)
        {
           var user= await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == _currentUserService.UserId, cancellationToken);
            command.User = user;
            return user is not null;
        }
    }
}
