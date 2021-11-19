using AutoMapper;
using AutoMapper.QueryableExtensions;
using Karolinska.Application.Dtos;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Queries
{
    public class GetProviderByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetProviderByIdQueryHandler : IQueryHandler<GetProviderByIdQuery, HealthcareProviderDto>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetProviderByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<HealthcareProviderDto?> HandleAsync(GetProviderByIdQuery query)
        {
            return await _context.HealthcareProviders.Where(e => e.Id.Equals(query.Id))
                .ProjectTo<HealthcareProviderDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        }
    }
}
