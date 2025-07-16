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
            var company = await _dbContext.Companies.FindAsync(request.CompanyId);
            
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



}
