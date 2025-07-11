using MediatR;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Features.ProjectContext.Commands
{
     public class AddEmployeToProject : IRequest<Unit>
    {
        public int ProjectId { get; set; }
        public List<int> UserIds { get; set; }
    }

    public class AddEmployeToProjectHandler : IRequestHandler<AddEmployeToProject, Unit>
    {
        private readonly ISibersDbContext _context;
        public AddEmployeToProjectHandler(ISibersDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddEmployeToProject request, CancellationToken cancellationToken)
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
