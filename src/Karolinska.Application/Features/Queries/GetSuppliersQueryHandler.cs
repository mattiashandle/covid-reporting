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
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Queries
{
    public class GetSuppliersQuery : PagedRequest
    {
        [Range(1, int.MaxValue / 1000)]
        public override int PageNumber { get; set; }
        
        [Range(1, 1000)]
        public override int PageSize { get; set; }
    }

    public class GetSuppliersQueryHandler : IQueryHandler<GetSuppliersQuery, PagedResponse<SupplierDto[]>>
    {
        private readonly KarolinskaContext _karolinskaContext;
        private readonly IMapper _mapper;

        public GetSuppliersQueryHandler(KarolinskaContext karolinskaContext, IMapper mapper)
        {
            _karolinskaContext = karolinskaContext ?? throw new ArgumentNullException(nameof(karolinskaContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResponse<SupplierDto[]>?> HandleAsync(GetSuppliersQuery query)
        {
            var baseQuery = _karolinskaContext.Suppliers.AsNoTracking();

            var totalRecords = baseQuery.Count();

            var totalPages = (double)totalRecords / query.PageSize;

            var queryResult = await baseQuery
                .OrderByDescending(e => e.Id)
                .ProjectTo<SupplierDto>(_mapper.ConfigurationProvider)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArrayAsync();

            var response = new PagedResponse<SupplierDto[]>(queryResult, query.PageNumber, query.PageSize)
            {
                TotalPages = (int)Math.Ceiling(totalPages),
                TotalRecords = totalRecords
            };

            return response;
        }
    }
}
