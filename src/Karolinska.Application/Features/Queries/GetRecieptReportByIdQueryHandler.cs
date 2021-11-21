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
    public class GetReceiptReportByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetRecieptReportByIdQueryHandler : IQueryHandler<GetReceiptReportByIdQuery, ReceiptReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetRecieptReportByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReceiptReportDto?> HandleAsync(GetReceiptReportByIdQuery query, CancellationToken cancellationToken)
        {
            return await _context.ReceiptReports.AsNoTracking()
                .Where(e => e.Id.Equals(query.Id))
                .ProjectTo<ReceiptReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
