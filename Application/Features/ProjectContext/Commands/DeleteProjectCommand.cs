using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;

namespace Application.Features.ProjectContext.Commands
{
    public class DeleteProjectCommand : IRequest<Unit>
    {
        public int ProjectId { get; set;    }
    }




    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Unit> 
    {
        private readonly ISibersDbContext _dbContext;
        public DeleteProjectCommandHandler(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _dbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new Exception("Project not found");
            }
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
