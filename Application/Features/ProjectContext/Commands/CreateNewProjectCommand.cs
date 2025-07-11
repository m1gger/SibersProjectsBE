using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.ProjectContext.Commands
{
    public class CreateNewProjectCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CustomerCompanyId { get; set; }
        public int ContractorCompanyId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }

        public int LeaderUserId { get; set; }
         
    }

    public class CreateNewProjectCommandHandler : IRequestHandler<CreateNewProjectCommand, Unit>
    {
        // Assuming you have a service to handle project creation
        private readonly ISibersDbContext _dbContext;
        public CreateNewProjectCommandHandler(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(CreateNewProjectCommand request, CancellationToken cancellationToken)
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
            project.ProjectCompanies.Add(projectCustomer);
            project.ProjectCompanies.Add(projectContractor);
           await _dbContext.Projects.AddAsync(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;


        }
    }
}
