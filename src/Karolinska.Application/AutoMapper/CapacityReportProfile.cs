using AutoMapper;
using Karolinska.Application.Dtos;
using Karolinska.Core.Entities;

namespace Karolinska.Application.AutoMapper
{
    public class CapacityReportProfile : Profile
    {
        public CapacityReportProfile()
        {
            CreateMap<CapacityReport, CapacityReportDto>();

            //TODO MOVE OUT
            CreateMap<ExpenditureReport, ExpenditureReportDto>();
            CreateMap<StockBalanceReport, StockBalanceReportDto>();
            CreateMap<OrderReport, OrderReportDto>();
            CreateMap<HealthcareProvider, HealthcareProviderDto>();
            CreateMap<ReceiptReport, ReceiptReportDto>();
        }
    }
}
