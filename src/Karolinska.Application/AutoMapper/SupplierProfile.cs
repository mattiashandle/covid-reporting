using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;

namespace Karolinska.Application.AutoMapper
{
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<Supplier, SupplierDto>();
        }
    }
}
