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
    public class GetCapacityReportByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetCapacityReportByIdQueryHandler : IQueryHandler<GetCapacityReportByIdQuery, CapacityReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetCapacityReportByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
}

        public async Task<CapacityReportDto?> HandleAsync(GetCapacityReportByIdQuery query, CancellationToken cancellationToken)
        {
            return await _context.CapacityReports.AsNoTracking()
                .Where(e => e.Id.Equals(query.Id))
                .ProjectTo<CapacityReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
