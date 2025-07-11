using MediatR;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Application.Common.AbstructClasses;
using Application.Common.Dto;
using Application.Features.CompanyContext.Dto;

namespace Application.Features.CompanyContext.Query
{
    public class GetAllCompaniesQuery : PagedQuery , IRequest<PagedDto<CompanyDto>>
    {
        public string? Search { get; set; }
        public string? SortBy { get; set; }
        public bool? SortDescending { get; set; }
    }

    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, PagedDto<CompanyDto>> 
    {
        private readonly ISibersDbContext _context;
        public GetAllCompaniesQueryHandler(ISibersDbContext context)
        {
            _context = context;
        }

        public async Task<PagedDto<CompanyDto>> Handle(GetAllCompaniesQuery query, CancellationToken cancellation) 
        {
            var companies = _context.Companies.AsQueryable();
            if (!string.IsNullOrEmpty(query.Search))
            {
                companies = companies.Where(c => c.Name.Contains(query.Search));
            }
            if (!string.IsNullOrEmpty(query.SortBy))
            {
                companies = query.SortDescending.HasValue && query.SortDescending.Value
                    ? companies.OrderByDescending(c => EF.Property<object>(c, query.SortBy))
                    : companies.OrderBy(c => EF.Property<object>(c, query.SortBy));
            }
            var totalCount = await companies.CountAsync(cancellation);
            var items = await companies
                .Skip((query.PageNumber-1) * query.PageSize)
                .Take(query.PageSize)
                .Select(c => new CompanyDto
                {
                    Id = c.Id,
                    Name = c.Name,
                   
                })
                .ToListAsync(cancellation);
            return new PagedDto<CompanyDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = query.PageNumber,
                PageSize = query.PageSize
            };
        }
    }



}
