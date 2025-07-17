using MediatR;
using Application.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.CompanyContext.Commands
{
    public class DeleteCompanyCommand : IRequest<Unit>
    {
        public int CompanyId { get; set; }
    }


    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, Unit>
    {
        private readonly ISibersDbContext _dbContext;
        public DeleteCompanyCommandHandler(ISibersDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Unit> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == request.CompanyId);

            _dbContext.Companies.Remove(company);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }

    public class DeleteCompanyValidator : AbstractValidator<DeleteCompanyCommand>
    {
        private readonly ISibersDbContext _context;
        public DeleteCompanyValidator(ISibersDbContext context)
        {
            _context = context;
            RuleFor(x => x.CompanyId)
                .MustAsync(CompanyMustExist)
                .WithMessage("Company not found")
                .WithErrorCode("404");
        }
        private async Task<bool> CompanyMustExist(int copmanyId, CancellationToken cancellation)
        {
            return await _context.Companies.AnyAsync(x => x.Id == copmanyId);
        }
    }

    public class DeleteCompanyCommandValidator : AbstractValidator<DeleteCompanyCommand>
    {
        private readonly ISibersDbContext _context;

        public DeleteCompanyCommandValidator(ISibersDbContext context)
        {
            _context = context;

            RuleFor(x => x.CompanyId)
                .NotEmpty()
                .WithMessage("CompanyId is required.")
                .WithErrorCode("3001");
            RuleFor(x => x.CompanyId)
                .MustAsync(CompanyMustExist)
                .WithMessage("Company not found.")
                .WithErrorCode("404");
            RuleFor(x => x.CompanyId)
                .MustAsync(CompanyMustNotHaveProjects)
                .WithMessage("Company cannot be deleted because it has associated projects.")
                .WithErrorCode("4001");

        }
        private async Task<bool> CompanyMustExist(int companyId, CancellationToken cancellation)
        {
            return await _context.Companies.AnyAsync(x => x.Id == companyId);
        }

        private async Task<bool> CompanyMustNotHaveProjects(int companyId, CancellationToken cancellation)
        {
            return !await _context.ProjectCompanies.AnyAsync(x =>x.CompanyId==companyId);
        }
    }
}
