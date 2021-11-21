using AutoMapper;
using AutoMapper.QueryableExtensions;
using Karolinska.Application.Dtos;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Queries
{
    public class GetStockBalanceReportsQuery : PagedQuery 
    {
        public Guid HealthcareProviderId { get; set; }
    }

    public class GetStockBalanceReportsQueryHandler : IQueryHandler<GetStockBalanceReportsQuery, PagedResponse<StockBalanceReportDto[]>>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetStockBalanceReportsQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResponse<StockBalanceReportDto[]>?> HandleAsync(GetStockBalanceReportsQuery query, CancellationToken cancellationToken)
        {
            var baseQuery = _context.StockBalanceReports.AsNoTracking();

            baseQuery = baseQuery.Where(e => e.HealthcareProviderId.Equals(query.HealthcareProviderId));

            var totalRecords = baseQuery.Count();

            var totalPages = (double)totalRecords / query.PageSize;

            var queryResult = await baseQuery
                .OrderByDescending(e => e.InsertDate)
                .ProjectTo<StockBalanceReportDto>(_mapper.ConfigurationProvider)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArrayAsync(cancellationToken);

            var response = new PagedResponse<StockBalanceReportDto[]>(queryResult, query.PageNumber, query.PageSize)
            {
                TotalPages = (int)Math.Ceiling(totalPages),
                TotalRecords = totalRecords
            };

            return response;
        }
    }
}
