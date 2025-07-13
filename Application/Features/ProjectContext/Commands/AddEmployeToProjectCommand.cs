using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Application.Features.ProjectContext.Commands
{
     public class AddEmployeToProjectCommand : IRequest<Unit>
    {
        public int ProjectId { get; set; }
        public List<int> UserIds { get; set; }
    }

    public class AddEmployeToProjectHandler : IRequestHandler<AddEmployeToProjectCommand, Unit>
    {
        private readonly ISibersDbContext _context;
        private readonly IFileService _fileService;
        public AddEmployeToProjectHandler(ISibersDbContext context)
        {
            _context = context;
           
        }
        public async Task<Unit> Handle(AddEmployeToProjectCommand request, CancellationToken cancellationToken)
        {
            var projectEmployers= new List<ProjectUsers>();
            foreach (var userId in request.UserIds)
            {
                var projectUser = new ProjectUsers
                {
                    ProjectId = request.ProjectId,
                    UserId = userId
                };
                projectEmployers.Add(projectUser);
            }
          
            await _context.ProjectUsers.AddRangeAsync(projectEmployers);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }

    }

    public class AddEmployeToProjectValidation : AbstractValidator<AddEmployeToProjectCommand>
    {
        private readonly ISibersDbContext _dbContext;

        public AddEmployeToProjectValidation(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.ProjectId)
                .NotEmpty()
                .WithMessage("Project ID is required.")
                .WithErrorCode("405");

            RuleFor(x => x.UserIds)
                .NotEmpty()
                .WithMessage("User IDs are required.")
                .WithErrorCode("405");

            RuleForEach(x => x.UserIds)
                .MustAsync(UserMustExist)
                .WithMessage("User not found")
                .WithErrorCode("404");


            RuleForEach(x => x.UserIds)
                .MustAsync(async (command, userId, cancellationToken) =>
                {
                    return !await _dbContext.ProjectUsers
                        .AnyAsync(pu => pu.UserId == userId && pu.ProjectId == command.ProjectId, cancellationToken);
                })
                .WithMessage("User is already assigned to the project")
                .WithErrorCode("409")
                .WithState((command, userId) => userId);
                }

        private async Task<bool> UserMustExist(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AnyAsync(x => x.Id == userId, cancellationToken);
        }

       
    }


}
