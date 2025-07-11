using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProjectContext.Commands
{
    public class CreateNewProjectCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CustomerCompanyId { get; set; }
        public int ContractorCompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }
        public int LeaderUserId { get; set; }
        public List<IFormFile>? ProjectDocument {  get; set; }
        public List<string>? FilesNames { get; set; } 

    }

    public class CreateNewProjectCommandHandler : IRequestHandler<CreateNewProjectCommand, int>
    {
        // Assuming you have a service to handle project creation
        private readonly ISibersDbContext _dbContext;
        private readonly IFileService _fileService;
        public CreateNewProjectCommandHandler(ISibersDbContext dbContext,IFileService fileService)
        {
            _dbContext = dbContext;
            _fileService = fileService;
        }
        public async Task<int> Handle(CreateNewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                Name = request.Name,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Priority = request.Priority,
                LeaderUserId = request.LeaderUserId
            };

            var projectCustomer = new ProjectCompany
            {
                CompanyId = request.CustomerCompanyId,
                Project = project,
                InProjectEnum = CompanyInProjectEnum.Custumer

            };
            var projectContractor = new ProjectCompany
            {
                CompanyId = request.ContractorCompanyId,
                Project = project,
                InProjectEnum = CompanyInProjectEnum.Contractor
            };
            if (request.ProjectDocument != null)
            {
                for (int i = 0; i < request.ProjectDocument.Count; i++)
                {
                    var description = request.FilesNames != null && i < request.FilesNames.Count
                        ? request.FilesNames[i]
                        : null;

                    var path = await _fileService.SaveFileAsync(
                        request.ProjectDocument[i],
                        description ?? Guid.NewGuid().ToString(), // fallback
                        request.Name);

                    var projectDocument = new ProjectDocument
                    {
                        Description = description,
                        Path = path,
                        Project = project
                    };
                    project.ProjectDocuments.Add(projectDocument);
                }
            }

            project.ProjectCompanies.Add(projectCustomer);
            project.ProjectCompanies.Add(projectContractor);
            await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return project.Id;


        }
    }
}
