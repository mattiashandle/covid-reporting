using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;

namespace Karolinska.Application.AutoMapper
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<CapacityReport, CapacityReportDto>();
            CreateMap<Supplier, SupplierDto>();
            CreateMap<ExpenditureReport, ExpenditureReportDto>();
            CreateMap<StockBalanceReport, StockBalanceReportDto>();
            CreateMap<OrderReport, OrderReportDto>();
            CreateMap<HealthcareProvider, HealthcareProviderDto>();
            CreateMap<ReceiptReport, ReceiptReportDto>();
        }
    }
}
