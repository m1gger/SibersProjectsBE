using MediatR;
using Application.Interfaces;
using FluentValidation;


namespace Application.Features.CompanyContext.Commands
{
    public class AddNewCompanyCommands : IRequest<Unit>
    {
        public string Name { get; set; }
    }

    public class AddNewCompanyCommandsHandler : IRequestHandler<AddNewCompanyCommands, Unit> 
    {
        private readonly ISibersDbContext _context;
        public AddNewCompanyCommandsHandler(ISibersDbContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(AddNewCompanyCommands request, CancellationToken cancellationToken)
        {
            var company = new Domain.Entities.Company
            {
                Name = request.Name
            };
            _context.Companies.Add(company);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }


    public class AddNewCompanyValidator : AbstractValidator<AddNewCompanyCommands>
    {
        public AddNewCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name is required.").WithErrorCode("3001")
                .MaximumLength(100).WithMessage("Company name cannot exceed 100 characters.").WithErrorCode("3002");
        }

    }
}
