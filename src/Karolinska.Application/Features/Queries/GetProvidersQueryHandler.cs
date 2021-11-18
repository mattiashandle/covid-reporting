using AutoMapper;
using AutoMapper.QueryableExtensions;
using Karolinska.Application.Dtos;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Queries
{
    public class GetProvidersQuery : PagedRequest
    {
        [Range(1, int.MaxValue / 1000)]
        public override int PageNumber { get; set; }

        [Range(1, 1000)]
        public override int PageSize { get; set; }
    }

    public class GetProvidersQueryHandler : IQueryHandler<GetProvidersQuery, PagedResponse<HealthcareProviderDto[]>>
    {
        private readonly KarolinskaContext _karolinskaContext;
        private readonly IMapper _mapper;

        public GetProvidersQueryHandler(KarolinskaContext karolinskaContext, IMapper mapper)
        {
            _karolinskaContext = karolinskaContext ?? throw new ArgumentNullException(nameof(karolinskaContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<PagedResponse<HealthcareProviderDto[]>?> HandleAsync(GetProvidersQuery query)
        {
            var baseQuery = _karolinskaContext.HealthcareProviders.AsNoTracking();

            var totalRecords = baseQuery.Count();

            var totalPages = (double)totalRecords / query.PageSize;

            var queryResult = await baseQuery
                .OrderByDescending(e => e.Id)
                .ProjectTo<HealthcareProviderDto>(_mapper.ConfigurationProvider)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArrayAsync();

            var response = new PagedResponse<HealthcareProviderDto[]>(queryResult, query.PageNumber, query.PageSize)
            {
                TotalPages = (int)Math.Ceiling(totalPages),
                TotalRecords = totalRecords
            };

            return response;
        }
    }
}
