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
    public class GetOrderReportByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetOrderReportByIdQueryHandler : IQueryHandler<GetOrderReportByIdQuery, OrderReportDto>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetOrderReportByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<OrderReportDto?> HandleAsync(GetOrderReportByIdQuery query, CancellationToken cancellationToken)
        {
            return await _context.OrderReports.AsNoTracking()
                .Where(e => e.Id.Equals(query.Id))
                .ProjectTo<OrderReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
