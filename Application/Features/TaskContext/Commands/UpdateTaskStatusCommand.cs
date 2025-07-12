using MediatR;
using Domain.Enums;
using Application.Interfaces;

namespace Application.Features.TaskContext.Commands
{
    public class UpdateTaskStatusCommand : IRequest<Unit>
    {
        public int TaskId { get; set; }
        public  TaskStatusEnum  Status { get; set; }
    }

    public class UpdateTaskStatusHandler : IRequestHandler<UpdateTaskStatusCommand, Unit>
    {
        private readonly ISibersDbContext _context;
        public UpdateTaskStatusHandler(ISibersDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.ProjectTasks.FindAsync(request.TaskId);
            if (task == null)
            {
                throw new Exception("Task not found");
            }
            task.TaskStatus = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

    

}
