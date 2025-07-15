using Microsoft.AspNetCore.Http;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Features.ProjectContext.Commands
{
    public class UpdateProjectCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Priority { get; set; }
        public int? LeaderUserId { get; set; }
        public List<IFormFile>? ProjectFilesToAdd { get; set; }
        public List<int>? ProjectFilesToDelete { get; set; } = new List<int>();

    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Unit> 
    {
        private readonly ISibersDbContext _context;
        private readonly IFileService _fileService;
        public UpdateProjectCommandHandler(ISibersDbContext dbContext, IFileService fileService)
        {
            _context = dbContext;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (project == null)
            {
                throw new Exception("Project not found");
            }

            if (!string.IsNullOrEmpty(command.Name))
                project.Name = command.Name;

            if (!string.IsNullOrEmpty(command.Description))
                project.Description = command.Description;

            if (command.StartDate.HasValue)
                project.StartDate = command.StartDate.Value;

            if (command.EndDate.HasValue)
                project.EndDate = command.EndDate.Value;

            if (command.Priority.HasValue)
                project.Priority = command.Priority.Value;

            if (command.LeaderUserId.HasValue)
                project.LeaderUserId = command.LeaderUserId.Value;

            // Добавляем файлы
            if (command.ProjectFilesToAdd != null && command.ProjectFilesToAdd.Any())
            {
                int i = 0;
                foreach (var file in command.ProjectFilesToAdd)
                {
                    var savedFile = await _fileService.SaveFileAsync(file,file.Name,project.Name);
                    var fileProject = new ProjectDocument { Description = file.Name,ProjectId=project.Id,Path=savedFile };
                    project.ProjectDocuments.Add(fileProject);
                }
            }

            // Удаляем файлы
            if (command.ProjectFilesToDelete != null && command.ProjectFilesToDelete.Any())
            {
                var filesToDelete = project.ProjectDocuments
                    .Where(f => command.ProjectFilesToDelete.Contains(f.Id))
                    .ToList();

                foreach (var file in filesToDelete)
                {
                 /// can add remove from file system at future
                    project.ProjectDocuments.Remove(file);
                }
            }

            _context.Projects.Update(project);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

    }



}
