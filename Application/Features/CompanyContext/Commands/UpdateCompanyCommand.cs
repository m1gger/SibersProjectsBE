using Application.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.CompanyContext.Commands
{
    public class UpdateCompanyCommand : IRequest<Unit>
    {
        public  int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }


    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Unit> 
    {
        private readonly ISibersDbContext _context;
        public UpdateCompanyCommandHandler(ISibersDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCompanyCommand command, CancellationToken cancellation) 
        {
            var company =await _context.Companies.FirstOrDefaultAsync(x => x.Id == command.CompanyId);
            company.Name = command.CompanyName;
            return Unit.Value;
        }
    }


    public class UpdateCompanyValidator : AbstractValidator<UpdateCompanyCommand> 
    {
        private readonly ISibersDbContext _context;
        public UpdateCompanyValidator(ISibersDbContext context)
        {

            _context = context;
            RuleFor(x => x.CompanyId)
                .MustAsync(CompanyMustExits)
                .WithErrorCode("404")
                .WithMessage("Company not found");
        }

        private async Task<bool> CompanyMustExits(int companyId, CancellationToken cancellation) 
        {
            return !await _context.Companies.AllAsync(x => x.Id == companyId);
        }
    }

}
