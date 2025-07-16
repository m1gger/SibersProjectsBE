using Application.Interfaces;
using MediatR;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
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
        public List<IFormFile>? ProjectDocument { get; set; }
        public List<string>? FilesNames { get; set; }

    }

    public class CreateNewProjectCommandHandler : IRequestHandler<CreateNewProjectCommand, int>
    {
        // Assuming you have a service to handle project creation
        private readonly ISibersDbContext _dbContext;
        private readonly IFileService _fileService;
        public CreateNewProjectCommandHandler(ISibersDbContext dbContext, IFileService fileService)
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

    public class CreateNewProjectCommandValidator : AbstractValidator<CreateNewProjectCommand>
    {
        private readonly ISibersDbContext _dbContext;
        public CreateNewProjectCommandValidator(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Project name is required.").WithErrorCode("5001");
            RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate)
                .WithMessage("Start date must be before or equal to end date.")
                .WithErrorCode("5002");
            RuleFor(x => x.Priority).InclusiveBetween(1, 5)
                .WithMessage("Priority must be between 1 and 5.")
                .WithErrorCode("5003");
            RuleFor(x => x.LeaderUserId).GreaterThan(0)
                .WithMessage("Leader user ID must be greater than 0.").WithErrorCode("5005");
            RuleFor(x => x.LeaderUserId)
                .MustAsync(UserMustExist).WithErrorCode("5006");
            RuleFor(x => x.CustomerCompanyId).MustAsync(CompanyMustExist)
                .WithMessage("Customer company not found.").WithErrorCode("5007");
            RuleFor(x => x.ContractorCompanyId).MustAsync(CompanyMustExist)
                .WithMessage("Contractor company not found.").WithErrorCode("5008");
        }

        private async Task<bool> UserMustExist(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AnyAsync(u => u.Id == userId, cancellationToken);
        }
        private async Task<bool> CompanyMustExist(int companyId, CancellationToken cancellationToken)
        {
            return await _dbContext.Companies.AnyAsync(c => c.Id == companyId, cancellationToken);
        }
    }
}
