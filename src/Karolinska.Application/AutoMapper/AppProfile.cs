using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;

namespace Karolinska.Application.AutoMapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<CapacityReport, CapacityReportDto>().ReverseMap();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<ExpenditureReport, ExpenditureReportDto>().ForMember(e => e.SupplierName, src => src.MapFrom(v => v.Supplier.Name));
            CreateMap<StockBalanceReport, StockBalanceReportDto>().ForMember(e => e.SupplierName, src => src.MapFrom(v => v.Supplier.Name));
            CreateMap<OrderReport, OrderReportDto>().ReverseMap();
            CreateMap<HealthcareProvider, HealthcareProviderDto>();
            CreateMap<ReceiptReport, ReceiptReportDto>().ForMember(e => e.SupplierName, src => src.MapFrom(v => v.Supplier.Name));
        }
    }
}
