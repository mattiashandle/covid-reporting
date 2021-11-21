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
    public class GetExpenditureReportByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetExpenditureReportByIdQueryHandler : IQueryHandler<GetExpenditureReportByIdQuery, ExpenditureReportDto?>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetExpenditureReportByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ExpenditureReportDto?> HandleAsync(GetExpenditureReportByIdQuery query, CancellationToken cancellationToken)
        {
            return await _context.ExpenditureReports.AsNoTracking()
                .Where(e => e.Id.Equals(query.Id))
                .ProjectTo<ExpenditureReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
