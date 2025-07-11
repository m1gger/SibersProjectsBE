using MediatR;
using Application.Interfaces;
using Domain.Entities;

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

}
