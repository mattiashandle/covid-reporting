using AutoMapper;
using AutoMapper.QueryableExtensions;
using Karolinska.Application.Dtos;
using Karolinska.Application.Wrappers;
using Karolinska.Core.Interfaces;
using Karolinska.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Karolinska.Application.Features.Queries
{
    public class GetHealthcareProvidersQuery : PagedQuery
    {

    }

    public class GetHealthcareProvidersQueryHandler : IQueryHandler<GetHealthcareProvidersQuery, PagedResponse<HealthcareProviderDto[]>>
    {
        private readonly KarolinskaContext _karolinskaContext;
        private readonly IMapper _mapper;

        public GetHealthcareProvidersQueryHandler(KarolinskaContext karolinskaContext, IMapper mapper)
        {
            _karolinskaContext = karolinskaContext ?? throw new ArgumentNullException(nameof(karolinskaContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<PagedResponse<HealthcareProviderDto[]>?> HandleAsync(GetHealthcareProvidersQuery query)
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
