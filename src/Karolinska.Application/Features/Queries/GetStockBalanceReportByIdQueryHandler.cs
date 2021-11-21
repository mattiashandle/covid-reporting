using AutoMapper;
using AutoMapper.QueryableExtensions;
using Karolinska.Application.Dtos;
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
    public class GetStockBalanceReportByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetStockBalanceReportByIdQueryHandler : IQueryHandler<GetStockBalanceReportByIdQuery, StockBalanceReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetStockBalanceReportByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<StockBalanceReportDto?> HandleAsync(GetStockBalanceReportByIdQuery query, CancellationToken cancellationToken)
        {
            return await _context.StockBalanceReports.AsNoTracking()
                .Where(e => e.Id.Equals(query.Id))
                .ProjectTo<StockBalanceReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
