using AutoMapper;
using AutoMapper.QueryableExtensions;
using Karolinska.Application.Dtos;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Queries
{
    public class GetOrderReportsQuery : PagedQuery
    {
        public Guid HealthcareProviderId { get; set; }
    }

    public class GetOrderReportsQueryHandler : IQueryHandler<GetOrderReportsQuery, PagedResponse<OrderReportDto[]>>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetOrderReportsQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResponse<OrderReportDto[]>?> HandleAsync(GetOrderReportsQuery query, CancellationToken cancellationToken)
        {
            var baseQuery = _context.OrderReports.AsNoTracking();

            baseQuery = baseQuery.Where(e => e.HealthcareProviderId.Equals(query.HealthcareProviderId));

            var totalRecords = baseQuery.Count();

            var totalPages = (double)totalRecords / query.PageSize;

            var queryResult = await baseQuery
                .OrderByDescending(e => e.InsertDate)
                .ProjectTo<OrderReportDto>(_mapper.ConfigurationProvider)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArrayAsync(cancellationToken);

            var response = new PagedResponse<OrderReportDto[]>(queryResult, query.PageNumber, query.PageSize)
            {
                TotalPages = (int)Math.Ceiling(totalPages),
                TotalRecords = totalRecords
            };

            return response;
        }
    }
}
