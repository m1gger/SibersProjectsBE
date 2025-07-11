using MediatR;
using Application.Interfaces;


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
}
