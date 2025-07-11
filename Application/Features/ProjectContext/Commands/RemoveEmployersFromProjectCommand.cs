using MediatR;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProjectContext.Commands
{
    public class RemoveEmployersFromProjectCommand : IRequest<Unit>
    {
        public int ProjectId { get; set; }
        public List<int> EmployersIds { get; set; }
    }

    public class RemoveEmployersFromProjectCommandHandler : IRequestHandler<RemoveEmployersFromProjectCommand, Unit> 
    {
        private readonly ISibersDbContext _context;
        public RemoveEmployersFromProjectCommandHandler(ISibersDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(RemoveEmployersFromProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectUsers)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
           
            var employersToRemove = project.ProjectUsers
                .Where(e => request.EmployersIds.Contains(e.UserId))
                .ToList();
            foreach (var employer in employersToRemove)
            {
                project.ProjectUsers.Remove(employer);
            }
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}
