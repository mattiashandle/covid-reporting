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
    public class GetSupplierByIdQuery
    {
        public Guid Id { get; set; }
    }

    public class GetSupplierByIdQueryHandler : IQueryHandler<GetSupplierByIdQuery, SupplierDto>
    {
        private readonly KarolinskaContext _context;
        private readonly IMapper _mapper;

        public GetSupplierByIdQueryHandler(KarolinskaContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SupplierDto?> HandleAsync(GetSupplierByIdQuery query)
        {
            return await _context.Suppliers.AsNoTracking()
                .Where(e => e.Id.Equals(query.Id))
                .ProjectTo<SupplierDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }
    }
}
